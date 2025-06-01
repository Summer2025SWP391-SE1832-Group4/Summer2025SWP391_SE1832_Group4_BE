using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class DoctorSchedule
    {
        public int ScheduleId { get; set; }

        public int DoctorId { get; set; }

        // 1=Monday, 2=Tuesday, ..., 7=Sunday (ISO 8601 standard)
        public int DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public ScheduleAvailability AvailabilityStatus { get; set; } =
            ScheduleAvailability.Available;

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public int? SlotDurationMinutes { get; set; }

        [MaxLength(255)]
        public string? Notes { get; set; }

        // Navigation properties
        public virtual Doctor Doctor { get; set; } = null!;
    }
}
