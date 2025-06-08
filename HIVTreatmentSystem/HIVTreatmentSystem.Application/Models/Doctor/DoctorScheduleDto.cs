using System;

namespace HIVTreatmentSystem.Application.Models.Doctor
{
    public class DoctorScheduleDto
    {
        public int ScheduleId { get; set; }
        public int DoctorId { get; set; }
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AvailabilityStatus { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? SlotDurationMinutes { get; set; }
        public string? Notes { get; set; }
    }
} 