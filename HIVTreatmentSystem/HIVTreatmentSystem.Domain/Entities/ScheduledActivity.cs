using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class ScheduledActivity
    {
        public int ScheduledActivityId { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; } = null!;

        public int? CreatedByStaffId { get; set; }

        [ForeignKey(nameof(CreatedByStaffId))]
        public virtual Staff? CreatedByStaff { get; set; }

        public DateTime ScheduledDate { get; set; }

        public ActivityType ActivityType { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public ActivityStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
