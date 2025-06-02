namespace HIVTreatmentSystem.API.Models.Auth
{
    public class SetPasswordRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
} 