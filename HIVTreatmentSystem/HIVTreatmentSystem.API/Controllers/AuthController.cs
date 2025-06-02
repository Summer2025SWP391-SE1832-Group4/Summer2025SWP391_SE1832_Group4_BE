using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.API.Models.Auth;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Settings;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
        private readonly IAuthenticateService _authenticateService;

        public AuthController(HIVDbContext context, IOptions<JwtSettings> jwtSettings, IAuthenticateService authenticateService)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _authenticateService = authenticateService;
        }

        /// <summary>
        /// Đăng nhập tài khoản.
        /// </summary>
        /// <param name="request">Thông tin đăng nhập.</param>
        /// <returns>Thông tin đăng nhập thành công hoặc lỗi.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            // Validate request
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and password are required");
            }

            // Find user by username
            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            // Check if user exists
            if (account == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Verify password (in a real app, you'd use a password hasher)
            // For demo purposes, we're checking directly
            // In production, use BCrypt or Identity password hasher
            if (!VerifyPassword(request.Password, account.PasswordHash))
            {
                return Unauthorized("Invalid username or password");
            }

            // Check if account is active
            if (account.AccountStatus != HIVTreatmentSystem.Domain.Enums.AccountStatus.Active)
            {
                return Unauthorized($"Account is {account.AccountStatus}. Please contact support.");
            }

            // Generate JWT token
            var token = GenerateJwtToken(account);

            // Update last login date
            account.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Create and return response
            return new LoginResponse
            {
                Token = token,
                RefreshToken = "", // Implement refresh token logic if needed
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                Username = account.Username,
                Email = account.Email,
                FullName = account.FullName,
                Role = account.Role.RoleName
            };
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

        // Simple password verification - replace with proper password hashing in production
        private bool VerifyPassword(string password, string passwordHash)
        {
            // In production, use BCrypt.Net.BCrypt.Verify() or similar
            // For demo purposes, we'll do a simple comparison (assuming passwordHash is plaintext)
            return password == passwordHash;
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

        [HttpPost("change-password/{id}")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, int id)
        {
            var result = await _authenticateService.ChangePassword(request.OldPassword, request.NewPassword, id);
            return Ok(new
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Message = "Change password successful"            
            });
        }
    }
}
