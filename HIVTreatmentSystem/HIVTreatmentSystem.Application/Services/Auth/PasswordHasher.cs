using HIVTreatmentSystem.Application.Interfaces;

namespace HIVTreatmentSystem.Application.Services.Auth
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(hash))
                return false;
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
} 