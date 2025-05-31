using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class SystemAuditLog
    {
        public long LogId { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public int? AccountId { get; set; }

        [MaxLength(50)]
        public string? Username { get; set; }

        [MaxLength(50)]
        public string? RoleAtTimeOfAction { get; set; }

        [Required]
        [MaxLength(255)]
        public string Action { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? AffectedEntity { get; set; }

        [MaxLength(100)]
        public string? AffectedEntityId { get; set; }

        [MaxLength(45)]
        public string? IpAddress { get; set; }

        public string? ActionDetails { get; set; }

        // Navigation properties
        public virtual Account? Account { get; set; }
    }
}
