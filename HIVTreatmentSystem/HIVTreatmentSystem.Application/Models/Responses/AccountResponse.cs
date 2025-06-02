using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class AccountResponse
    {
        public int AccountId { get; set; }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public AccountStatus AccountStatus { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
