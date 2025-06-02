using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Auth
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
} 