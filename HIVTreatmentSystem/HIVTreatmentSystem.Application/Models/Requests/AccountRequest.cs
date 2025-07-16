
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class AccountRequest
    {
        public string Username { get; set; } = default!;
        public string? PasswordHash { get; set; }
        public string Email { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string FullName { get; set; } = default!;
        public AccountStatus AccountStatus { get; set; }
        public int RoleId { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
