using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HIVTreatmentSystem.Application.Models.Settings;
using HIVTreatmentSystem.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HIVTreatmentSystem.Application.Services.Auth
{
    /// <summary>
    /// Service sinh JWT token cho các loại tài khoản.
    /// </summary>
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Sinh token cho bác sĩ.
        /// </summary>
        /// <param name="doctor">Đối tượng bác sĩ.</param>
        /// <returns>JWT token.</returns>
        public string GenerateToken(Doctor doctor)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
            );
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, doctor.DoctorId.ToString()),
                new Claim(ClaimTypes.Name, doctor.Account?.FullName ?? "Doctor"),
                new Claim(ClaimTypes.Email, doctor.Account?.Email ?? ""),
                new Claim(ClaimTypes.Role, "Doctor"),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Sinh token cho bệnh nhân.
        /// </summary>
        /// <param name="patient">Đối tượng bệnh nhân.</param>
        /// <returns>JWT token.</returns>
        public string GenerateToken(Patient patient)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
            );
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, patient.PatientId.ToString()),
                new Claim(ClaimTypes.Name, patient.Account?.FullName ?? "Patient"),
                new Claim(ClaimTypes.Email, patient.Account?.Email ?? ""),
                new Claim(ClaimTypes.Role, "Patient"),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Sinh token cho tài khoản bất kỳ.
        /// </summary>
        /// <param name="account">Đối tượng tài khoản.</param>
        /// <returns>JWT token.</returns>
        public string GenerateToken(Domain.Entities.Account account)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
            );
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            string role = "User";
            if (account.Doctor != null)
                role = "Doctor";
            else if (account.Patient != null)
                role = "Patient";
            else if (account.Staff != null)
                role = "Staff";

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Name, account.FullName ?? account.Username),
                new Claim(ClaimTypes.Email, account.Email ?? ""),
                new Claim(ClaimTypes.Role, role),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
