namespace HIVTreatmentSystem.API.Models.Auth
{
    public class LoginRequest
    {
        public string Email { get; set; } // Only email is used for login
        public string Password { get; set; }
    }
}

