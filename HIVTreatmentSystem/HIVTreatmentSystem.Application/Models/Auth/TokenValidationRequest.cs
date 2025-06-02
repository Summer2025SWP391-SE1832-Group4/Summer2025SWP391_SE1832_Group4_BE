using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Auth
{
    public class TokenValidationRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
} 