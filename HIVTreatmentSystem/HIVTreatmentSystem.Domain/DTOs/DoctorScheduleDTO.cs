using System;

namespace HIVTreatmentSystem.Domain.DTOs
{
    public class CreateMonthlyScheduleDTO
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Month { get; set; }
        public int? SlotDurationMinutes { get; set; }
        public string? Notes { get; set; }
    }
} 