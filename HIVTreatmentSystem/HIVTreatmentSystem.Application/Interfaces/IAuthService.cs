using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Models.Auth;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse> LoginAsync(LoginRequest request);
        Task<ApiResponse> RegisterAsync(RegisterRequest request);
        Task<ApiResponse> SetPasswordAsync(SetPasswordRequest request);
        Task<ApiResponse> GetRolesAsync();
        Task<TokenValidationResponse> ValidateTokenAsync(string token);
        Task<bool> ChangePassword(string oldPassword, string newPassword, int id);
    }
} 