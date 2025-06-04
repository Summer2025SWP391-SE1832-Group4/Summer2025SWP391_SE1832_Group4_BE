using System;
using Swashbuckle.AspNetCore.Annotations;

namespace HIVTreatmentSystem.Application.Models.DoctorSchedule
{
    /// <summary>
    /// Data Transfer Object for Doctor Schedule.
    /// </summary>
    public class DoctorScheduleDto
    {
        /// <summary>
        /// Doctor's unique identifier.
        /// </summary>
        [SwaggerSchema(Description = "Doctor's unique identifier.")]
        public int DoctorId { get; set; }

        /// <summary>
        /// Day of the week (1=Monday, ..., 7=Sunday).
        /// </summary>
        [SwaggerSchema(Description = "Day of the week (1=Monday, ..., 7=Sunday).")]
        public int DayOfWeek { get; set; }

        /// <summary>
        /// Start time of the working schedule (format: HH:mm:ss).
        /// </summary>
        [SwaggerSchema(Description = "Start time of the working schedule (format: HH:mm:ss)")]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// End time of the working schedule (format: HH:mm:ss).
        /// </summary>
        [SwaggerSchema(Description = "End time of the working schedule (format: HH:mm:ss)")]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Availability status of the schedule.
        /// </summary>
        [SwaggerSchema(Description = "Availability status of the schedule.")]
        public int AvailabilityStatus { get; set; }

        /// <summary>
        /// The date from which this schedule is effective.
        /// </summary>
        [SwaggerSchema(Description = "The date from which this schedule is effective.")]
        public DateTime EffectiveFrom { get; set; }

        /// <summary>
        /// The date until which this schedule is effective (optional).
        /// </summary>
        [SwaggerSchema(Description = "The date until which this schedule is effective (optional).")]
        public DateTime? EffectiveTo { get; set; }

        /// <summary>
        /// Duration of each slot in minutes (optional).
        /// </summary>
        [SwaggerSchema(Description = "Duration of each slot in minutes (optional).")]
        public int? SlotDurationMinutes { get; set; }

        /// <summary>
        /// Additional notes for the schedule (optional).
        /// </summary>
        [SwaggerSchema(Description = "Additional notes for the schedule (optional).")]
        public string? Notes { get; set; }
    }
} 