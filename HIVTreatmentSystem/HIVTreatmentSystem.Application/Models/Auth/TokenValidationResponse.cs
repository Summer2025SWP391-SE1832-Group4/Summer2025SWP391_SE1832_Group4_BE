using System;

namespace HIVTreatmentSystem.Application.Models.Auth
{
    public class TokenValidationResponse
    {
        public bool IsValid { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public DateTime? Expiration { get; set; }
        public string? ErrorMessage { get; set; }
    }
} 