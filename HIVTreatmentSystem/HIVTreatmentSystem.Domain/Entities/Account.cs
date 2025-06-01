using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Account
    {
        public int AccountId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;

        public int RoleId { get; set; }

        public DateTime? LastLoginAt { get; set; }

        [MaxLength(255)]
        public string? ProfileImageUrl { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        // Navigation properties
        public virtual Role Role { get; set; } = null!;
        public virtual Patient? Patient { get; set; }
        public virtual Doctor? Doctor { get; set; }
        public virtual Staff? Staff { get; set; }
        public virtual ICollection<Appointment> CreatedAppointments { get; set; } =
            new List<Appointment>();
        public virtual ICollection<EducationalMaterial> AuthoredMaterials { get; set; } =
            new List<EducationalMaterial>();
        public virtual ICollection<SystemAuditLog> AuditLogs { get; set; } =
            new List<SystemAuditLog>();
    }
}
