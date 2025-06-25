using System;
using System.Security.Claims;
using System.Threading.Tasks;
using HIVTreatmentSystem.API.Models.Auth;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Auth;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Settings;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Infrastructure.Data;
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
            if (
                result.Message == "Invalid email or password"
                || result.Message.Contains("required")
            )
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
        public async Task<ActionResult<TokenValidationResponse>> ValidateToken(
            TokenValidationRequest request
        )
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest(
                    new TokenValidationResponse
                    {
                        IsValid = false,
                        ErrorMessage = "Token is required",
                    }
                );
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
            try
            {
                var result = await _authService.RegisterAsync(request);
                // Check for duplicate data errors first
                if (result.Message.Contains("already exists") || result.Message.Contains("already in use"))
                {
                    return StatusCode(400, new ApiResponse(result.Message));
                }
                // Then check for other errors
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ApiResponse(ex.Message));
            }
        }
        
        /// <summary>
        /// Register a new account by admin.
        /// </summary>
        /// <param name="request">Registration information by admin.</param>
        /// <returns>Registration result.</returns>
        [HttpPost("register-by-admin")]
        public async Task<ActionResult<ApiResponse>> RegisterByAdmin(RegisterByAdminRequest request)
        {
            try
            {
                var result = await _authService.RegisterByAdminAsync(request);
                // Check for duplicate data errors first
                if (result.Message.Contains("already exists") || result.Message.Contains("already in use"))
                {
                    return StatusCode(400, new ApiResponse(result.Message));
                }
                // Then check for other errors
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ApiResponse(ex.Message));
            }
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

        // [HttpGet("profile")]
        // [Authorize]
        // public ActionResult GetUserProfile()
        // {
        //     return Ok(new
        //     {
        //         UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
        //         Username = User.FindFirstValue(ClaimTypes.Name),
        //         Email = User.FindFirstValue(ClaimTypes.Email),
        //         Role = User.FindFirstValue(ClaimTypes.Role),
        //         IsAuthenticated = User.Identity?.IsAuthenticated ?? false
        //     });
        // }
        [HttpPost("change-password/{id}")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, int id)
        {
            var result = await _authService.ChangePassword(request.OldPassword, request.NewPassword, id);
            var response = new ApiResponse(result.Message);

            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _authService.ForgotPasswordAsync(request.Email);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request.Token, request.NewPassword);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
