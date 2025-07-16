

namespace HIVTreatmentSystem.Application.Models.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public int AccountID { get; set; } = 0;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string profileImageUrl { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? DoctorSpecialty { get; set; } = null!;

        // Add patient-specific properties
        public string? PatientCodeAtFacility { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? ConsentInformation { get; set; }
        public string? AdditionalNotes { get; set; }
    }
} 