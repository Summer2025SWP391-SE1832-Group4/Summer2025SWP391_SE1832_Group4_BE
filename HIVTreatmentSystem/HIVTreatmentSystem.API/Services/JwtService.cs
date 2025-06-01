using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HIVTreatmentSystem.API.Models; // Đảm bảo JwtSettings được định nghĩa ở đây
using HIVTreatmentSystem.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HIVTreatmentSystem.API.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        // Phương thức private helper để tránh lặp code
        private string GenerateTokenInternal(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes), // Sử dụng UtcNow
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(Doctor doctor)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, doctor.DoctorId.ToString()),
                new Claim(ClaimTypes.Name, doctor.Account?.FullName ?? "Doctor"),
                new Claim(ClaimTypes.Email, doctor.Account?.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, "Doctor")
            };
            return GenerateTokenInternal(claims);
        }

        public string GenerateToken(Patient patient)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, patient.PatientId.ToString()),
                new Claim(ClaimTypes.Name, patient.Account?.FullName ?? "Patient"),
                new Claim(ClaimTypes.Email, patient.Account?.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, "Patient")
            };
            return GenerateTokenInternal(claims);
        }

        // Cân nhắc: Nếu bạn cũng cần tạo token trực tiếp từ Account trong API layer,
        // bạn có thể thêm một phương thức tương tự như trong Application.JwtService
        // public string GenerateToken(Account account)
        // {
        //     // ... logic tương tự như Application.JwtService ...
        //     // string role = DetermineRoleBasedOnAccount(account); // Helper để xác định vai trò
        //     var claims = new[]
        //     {
        //         new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
        //         new Claim(ClaimTypes.Name, account.FullName ?? account.Username),
        //         new Claim(ClaimTypes.Email, account.Email ?? string.Empty),
        //         // new Claim(ClaimTypes.Role, role)
        //     };
        //     return GenerateTokenInternal(claims);
        // }
    }
}