using System;

namespace HIVTreatmentSystem.Application.Models.DoctorSchedule
{
    public class CreateWeeklyScheduleDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int? SlotDurationMinutes { get; set; }
        public string? Notes { get; set; }
    }
} 