using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.API.Models.Auth;
using HIVTreatmentSystem.Application.Models.Settings;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Services;


namespace HIVTreatmentSystem.API.Controllers
{
    /// <summary>
    /// Controller xử lý xác thực và đăng nhập cho người dùng.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HIVDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailService;
        public AuthController(HIVDbContext context, IOptions<JwtSettings> jwtSettings, IEmailService emailService)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
        }

        /// <summary>
        /// Đăng nhập tài khoản.
        /// </summary>
        /// <param name="request">Thông tin đăng nhập.</param>
        /// <returns>Thông tin đăng nhập thành công hoặc lỗi.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> Login(LoginRequest request)
        {
            // Validate request
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new ApiResponse("Email and password are required"));
            }

            // Find user by email only
            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            // Check if user exists
            if (account == null)
            {
                return Unauthorized(new ApiResponse("Invalid email or password"));
            }

            // Verify password (in a real app, you'd use a password hasher)
            if (!VerifyPassword(request.Password, account.PasswordHash))
            {
                return Unauthorized(new ApiResponse("Invalid email or password"));
            }

            // Check if account is active
            if (account.AccountStatus != HIVTreatmentSystem.Domain.Enums.AccountStatus.Active)
            {
                return Unauthorized(new ApiResponse($"Account is {account.AccountStatus}. Please contact support."));
            }

            // Generate JWT token
            var token = GenerateJwtToken(account);

            // Update last login date
            account.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Create and return response
            var loginResponse = new LoginResponse
            {
                Token = token,
                RefreshToken = "", // Implement refresh token logic if needed
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                Username = account.Username,
                Email = account.Email,
                FullName = account.FullName,
                Role = account.Role.RoleName
            };
            return Ok(new ApiResponse("Login successful", loginResponse));
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của token.
        /// </summary>
        /// <returns>Thông tin xác thực token.</returns>
        [HttpGet("validate")]
        [Authorize]
        public ActionResult ValidateToken()
        {
            return Ok(new 
            { 
                IsValid = true,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Username = User.FindFirstValue(ClaimTypes.Name),
                Role = User.FindFirstValue(ClaimTypes.Role)
            });
        }

        /// <summary>
        /// Register a new account.
        /// </summary>
        /// <param name="request">Registration information.</param>
        /// <returns>Registration result.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.FullName))
            {
                return BadRequest(new ApiResponse("Please provide all required information."));
            }

            if (await _context.Accounts.AnyAsync(a => a.Username == request.Username))
            {
                return Conflict(new ApiResponse("Username already exists."));
            }
            if (await _context.Accounts.AnyAsync(a => a.Email == request.Email))
            {
                return Conflict(new ApiResponse("Email is already in use."));
            }

            // Generate password set token and expiry (30 minutes)
            var token = Guid.NewGuid().ToString();
            var expiry = DateTime.UtcNow.AddMinutes(30);

            // Create new account with empty password hash
            var account = new Account
            {
                Username = request.Username,
                PasswordHash = string.Empty, // No password yet
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                RoleId = request.RoleId,
                CreatedAt = DateTime.UtcNow,
                AccountStatus = HIVTreatmentSystem.Domain.Enums.AccountStatus.PendingVerification,
                PasswordResetToken = token,
                PasswordResetTokenExpiry = expiry
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            // Send email with password set link
            var setPasswordUrl = $"https://your-frontend.com/set-password?token={token}";
            var subject = "Set your password for HIV Treatment System";
            var body = $"<p>Hello {account.FullName},</p>" +
                       $"<p>Thank you for registering. Please set your password by clicking the link below (valid for 30 minutes):</p>" +
                       $"<p><a href='{setPasswordUrl}'>Set Password</a></p>" +
                       $"<p>If you did not request this, please ignore this email.</p>";
            await _emailService.SendEmailAsync(account.Email, subject, body);

            return Ok(new ApiResponse("Registration successful! Please check your email to set your password.", new { account.AccountId, account.Username, account.Email, account.FullName, account.RoleId }));
        }

        /// <summary>
        /// Set a new password using the token sent via email after registration.
        /// </summary>
        /// <param name="request">Set password request (token, new password).</param>
        /// <returns>Result of password set.</returns>
        [HttpPost("set-password")]
        public async Task<ActionResult<ApiResponse>> SetPassword(SetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest(new ApiResponse("Token and new password are required."));
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.PasswordResetToken == request.Token);
            if (account == null)
            {
                return BadRequest(new ApiResponse("Invalid or expired token."));
            }
            if (!account.PasswordResetTokenExpiry.HasValue || account.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                return BadRequest(new ApiResponse("Token has expired. Please request a new password reset."));
            }

            // Hash and set new password
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            account.PasswordResetToken = null;
            account.PasswordResetTokenExpiry = null;
            account.AccountStatus = HIVTreatmentSystem.Domain.Enums.AccountStatus.Active;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse("Password has been set successfully. You can now log in."));
        }

        /// <summary>
        /// Get all available roles.
        /// </summary>
        /// <returns>List of roles.</returns>
        [HttpGet("roles")]
        public async Task<ActionResult<ApiResponse>> GetRoles()
        {
            var roles = await _context.Roles
                .Select(r => new { r.RoleId, r.RoleName, r.Description })
                .ToListAsync();
            return Ok(new ApiResponse("Success", roles));
        }

        /// <summary>
        /// Logout the current user (client should remove the token).
        /// </summary>
        /// <returns>Logout result.</returns>
        [HttpPost("logout")]
        [Authorize]
        public ActionResult<ApiResponse> Logout()
        {
            // No server-side action needed for JWT logout.
            return Ok(new ApiResponse("Logout successful."));
        }

        // Simple password verification - now using BCrypt
        private bool VerifyPassword(string password, string passwordHash)
        {
            // Use BCrypt to verify hashed password
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        private string GenerateJwtToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role.RoleName)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
