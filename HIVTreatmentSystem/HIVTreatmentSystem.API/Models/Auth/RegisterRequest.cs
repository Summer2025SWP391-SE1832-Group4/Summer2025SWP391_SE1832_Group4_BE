namespace HIVTreatmentSystem.API.Models.Auth
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public int RoleId { get; set; } // Role to assign (e.g. Patient, Doctor, etc.)
    }
} 