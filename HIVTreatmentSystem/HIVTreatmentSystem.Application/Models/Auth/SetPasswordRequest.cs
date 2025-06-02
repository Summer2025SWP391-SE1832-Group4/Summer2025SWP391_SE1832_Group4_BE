using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Auth
{
    public class SetPasswordRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
} 