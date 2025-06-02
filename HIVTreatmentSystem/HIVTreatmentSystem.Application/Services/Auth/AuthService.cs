using BCrypt.Net;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Auth;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Models.Settings;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailService;
        private readonly JwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(
            IAccountRepository accountRepository,
            IOptions<JwtSettings> jwtSettings,
            IEmailService emailService,
            JwtService jwtService,
            IPasswordHasher passwordHasher
        )
        {
            _accountRepository = accountRepository;
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApiResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return new ApiResponse("Email and password are required");
            }

            var account = await _accountRepository.GetByEmailAsync(request.Email);
            if (account == null)
            {
                return new ApiResponse("Invalid email or password");
            }

            if (!_passwordHasher.VerifyPassword(request.Password, account.PasswordHash))
            {
                return new ApiResponse("Invalid email or password");
            }

            if (account.AccountStatus != AccountStatus.Active)
            {
                return new ApiResponse(
                    $"Account is {account.AccountStatus}. Please contact support."
                );
            }

            var token = _jwtService.GenerateToken(account);
            account.LastLoginAt = DateTime.UtcNow;
            await _accountRepository.SaveChangesAsync();

            var loginResponse = new LoginResponse
            {
                Token = token,
                RefreshToken = "",
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                Username = account.Username,
                Email = account.Email,
                FullName = account.FullName,
                Role = account.Role.RoleName,
            };
            return new ApiResponse("Login successful", loginResponse);
        }

        public async Task<ApiResponse> RegisterAsync(RegisterRequest request)
        {
            if (
                string.IsNullOrWhiteSpace(request.Username)
                || string.IsNullOrWhiteSpace(request.Email)
                || string.IsNullOrWhiteSpace(request.FullName)
            )
            {
                return new ApiResponse("Please provide all required information.");
            }

            if (await _accountRepository.UsernameExistsAsync(request.Username))
            {
                return new ApiResponse("Username already exists.");
            }
            if (await _accountRepository.EmailExistsAsync(request.Email))
            {
                return new ApiResponse("Email is already in use.");
            }

            var token = Guid.NewGuid().ToString();
            var expiry = DateTime.UtcNow.AddMinutes(30);

            var account = new Domain.Entities.Account
            {
                Username = request.Username,
                PasswordHash = string.Empty,
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                RoleId = request.RoleId,
                CreatedAt = DateTime.UtcNow,
                AccountStatus = AccountStatus.PendingVerification,
                PasswordResetToken = token,
                PasswordResetTokenExpiry = expiry,
            };

            await _accountRepository.AddAsync(account);
            await _accountRepository.SaveChangesAsync();

            var setPasswordUrl = $"https://hivtreatment.vercel.app/passwordAfterRegister-page?token={token}";
            var subject = "Set your password for HIV Treatment System";
            var body =
                $"<p>Hello {account.FullName},</p>"
                + $"<p>Thank you for registering. Please set your password by clicking the link below (valid for 30 minutes):</p>"
                + $"<p><a href='{setPasswordUrl}'>Set Password</a></p>"
                + $"<p>If you did not request this, please ignore this email.</p>";
            await _emailService.SendEmailAsync(account.Email, subject, body);

            return new ApiResponse(
                "Registration successful! Please check your email to set your password.",
                new
                {
                    account.AccountId,
                    account.Username,
                    account.Email,
                    account.FullName,
                    account.RoleId,
                }
            );
        }

        public async Task<ApiResponse> SetPasswordAsync(SetPasswordRequest request)
        {
            if (
                string.IsNullOrWhiteSpace(request.Token)
                || string.IsNullOrWhiteSpace(request.NewPassword)
            )
            {
                return new ApiResponse("Token and new password are required.");
            }

            var accounts = await _accountRepository.FindAsync(a =>
                a.PasswordResetToken == request.Token
            );
            var account = accounts.FirstOrDefault();
            if (account == null)
            {
                return new ApiResponse("Invalid or expired token.");
            }
            if (
                !account.PasswordResetTokenExpiry.HasValue
                || account.PasswordResetTokenExpiry < DateTime.UtcNow
            )
            {
                return new ApiResponse("Token has expired. Please request a new password reset.");
            }

            account.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            account.PasswordResetToken = null;
            account.PasswordResetTokenExpiry = null;
            account.AccountStatus = AccountStatus.Active;
            await _accountRepository.SaveChangesAsync();

            return new ApiResponse("Password has been set successfully. You can now log in.");
        }

        public async Task<ApiResponse> GetRolesAsync()
        {
            var roles = await _accountRepository.GetAllRolesAsync();
            return new ApiResponse(
                "Success",
                roles
                    .Select(r => new
                    {
                        r.RoleId,
                        r.RoleName,
                        r.Description,
                    })
                    .ToList()
            );
        }

        public async Task<TokenValidationResponse> ValidateTokenAsync(string token)
        {
            var response = new TokenValidationResponse();

            try
            {
                // Thiết lập các tham số xác thực token
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
                    ),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero,
                };

                // Thực hiện xác thực token
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(
                    token,
                    tokenValidationParameters,
                    out var validatedToken
                );
                var jwtToken = validatedToken as JwtSecurityToken;

                if (jwtToken == null)
                {
                    response.IsValid = false;
                    response.ErrorMessage = "Invalid token format";
                    return response;
                }

                // Token hợp lệ, trích xuất thông tin người dùng
                response.IsValid = true;
                response.UserId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                response.Username = principal.FindFirst(ClaimTypes.Name)?.Value;
                response.Email = principal.FindFirst(ClaimTypes.Email)?.Value;
                response.Role = principal.FindFirst(ClaimTypes.Role)?.Value;
                response.Expiration = jwtToken.ValidTo;
            }
            catch (SecurityTokenExpiredException)
            {
                response.IsValid = false;
                response.ErrorMessage = "Token has expired";
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                response.IsValid = false;
                response.ErrorMessage = "Invalid token signature";
            }
            catch (Exception ex)
            {
                response.IsValid = false;
                response.ErrorMessage = $"Token validation failed: {ex.Message}";
            }

            return response;
        }

        public async Task<ChangePasswordResponse> ChangePassword(string oldPassword, string newPassword, int id)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(id);
                if (account == null)
                    return new ChangePasswordResponse { Success = false, Message = "Account not found." };
                bool checkPassword = _passwordHasher.VerifyPassword(oldPassword, account.PasswordHash);
                if (!checkPassword)
                    return new ChangePasswordResponse { Success = false, Message = "Old password is incorrect." };
                bool isSamePassword = _passwordHasher.VerifyPassword(newPassword, account.PasswordHash);
                if (isSamePassword)
                    return new ChangePasswordResponse { Success = false, Message = "New password must be different from the old password." };
                account.PasswordHash = _passwordHasher.HashPassword(newPassword);
                _accountRepository.Update(account);
                return new ChangePasswordResponse { Success = true, Message = "Password changed successfully." };

            }
            catch (Exception ex)
            {
                return new ChangePasswordResponse { Success = false, Message = "An error occurred while changing the password." };
            }
        }

        public async Task<ApiResponse> ForgotPasswordAsync(string email)
        {
            var account = await _accountRepository.GetByEmailAsync(email);
            if (account == null)
                return new ApiResponse("Email not found.");

            if (string.IsNullOrEmpty(account.PasswordHash))
                return new ApiResponse("Please verify your account first.");

            string token = Guid.NewGuid().ToString();
            account.PasswordResetToken = token;
            account.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);

            _accountRepository.Update(account);

            string resetLink = $"https://your-frontend.com/set-password?token={token}";
            await _emailService.SendEmailAsync(email, "Reset Your Password", $"Click the link to reset: {resetLink}");

            return new ApiResponse("Password reset link has been sent to your email.");
        }

        public async Task<ApiResponse> ResetPasswordAsync(string token, string newPassword)
        {
            var account = await _accountRepository.GetByResetTokenAsync(token);
            if (account == null || account.PasswordResetTokenExpiry < DateTime.UtcNow)
                return new ApiResponse("Invalid or expired password reset token.");

            account.PasswordHash = _passwordHasher.HashPassword(newPassword);
            account.PasswordResetToken = null;
            account.PasswordResetTokenExpiry = null;

            _accountRepository.Update(account);

            return new ApiResponse("Password has been reset successfully.");
        }


    }
}
