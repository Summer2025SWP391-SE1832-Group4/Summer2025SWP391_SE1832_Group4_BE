using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.API.Models.Auth
{
    public class ChangePasswordRequest
    {
        [Required]
        public string? OldPassword { get; set; }

        [Required]
        public string? NewPassword { get; set; }
    }
}
