using System;
using System.Security.Claims;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    /// <summary>
    /// Controller xử lý xác thực và đăng nhập cho người dùng.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Đăng nhập tài khoản.
        /// </summary>
        /// <param name="request">Thông tin đăng nhập.</param>
        /// <returns>Thông tin đăng nhập thành công hoặc lỗi.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (result.Message == "Invalid email or password" || result.Message.Contains("required"))
                return Unauthorized(result);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của token.
        /// </summary>
        /// <returns>Thông tin xác thực token.</returns>
        [HttpPost("validate")]
        public async Task<ActionResult<TokenValidationResponse>> ValidateToken(TokenValidationRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest(new TokenValidationResponse
                {
                    IsValid = false,
                    ErrorMessage = "Token is required"
                });
            }

            var result = await _authService.ValidateTokenAsync(request.Token);
            if (!result.IsValid)
                return BadRequest(result);
            
            return Ok(result);
        }

        /// <summary>
        /// Register a new account.
        /// </summary>
        /// <param name="request">Registration information.</param>
        /// <returns>Registration result.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Set a new password using the token sent via email after registration.
        /// </summary>
        /// <param name="request">Set password request (token, new password).</param>
        /// <returns>Result of password set.</returns>
        [HttpPost("set-password")]
        public async Task<ActionResult<ApiResponse>> SetPassword(SetPasswordRequest request)
        {
            var result = await _authService.SetPasswordAsync(request);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get all available roles.
        /// </summary>
        /// <returns>List of roles.</returns>
        [HttpGet("roles")]
        public async Task<ActionResult<ApiResponse>> GetRoles()
        {
            var result = await _authService.GetRolesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Logout the current user (client should remove the token).
        /// </summary>
        /// <returns>Logout result.</returns>
        [HttpPost("logout")]
        [Authorize]
        public ActionResult<ApiResponse> Logout()
        {
            return Ok(new ApiResponse("Logout successful."));
        }

        [HttpGet("profile")]
        [Authorize]
        public ActionResult GetUserProfile()
        {
            return Ok(new
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Username = User.FindFirstValue(ClaimTypes.Name),
                Email = User.FindFirstValue(ClaimTypes.Email),
                Role = User.FindFirstValue(ClaimTypes.Role),
                IsAuthenticated = User.Identity?.IsAuthenticated ?? false
            });
        }
    }
}
