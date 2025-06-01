using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class AccountRequest
    {
        [Required, MaxLength(50)]
        public string Username { get; set; } = default!;

        [MaxLength(255)]
        public string? PasswordHash { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = default!;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = default!;

        [Required]
        public AccountStatus AccountStatus { get; set; }

        [Required]
        public int RoleId { get; set; }

        [MaxLength(255)]
        public string? ProfileImageUrl { get; set; }
    }
}
