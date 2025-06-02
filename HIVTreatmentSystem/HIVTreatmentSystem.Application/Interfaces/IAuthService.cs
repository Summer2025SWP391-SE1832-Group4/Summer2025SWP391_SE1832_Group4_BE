using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Models.Auth;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse> LoginAsync(LoginRequest request);
        Task<ApiResponse> RegisterAsync(RegisterRequest request);
        Task<ApiResponse> SetPasswordAsync(SetPasswordRequest request);
        Task<ApiResponse> GetRolesAsync();
        Task<TokenValidationResponse> ValidateTokenAsync(string token);
        Task<ChangePasswordResponse> ChangePassword(string oldPassword, string newPassword, int id);
        Task<ApiResponse> ForgotPasswordAsync(string email);
        Task<ApiResponse> ResetPasswordAsync(string token, string newPassword);
    }
} 