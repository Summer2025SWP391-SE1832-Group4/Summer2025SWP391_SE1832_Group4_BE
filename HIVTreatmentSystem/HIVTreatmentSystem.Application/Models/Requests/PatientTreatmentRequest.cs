using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    /// <summary>
    /// Request model for creating or updating a patient treatment
    /// </summary>
    public class PatientTreatmentRequest
    {
        /// <summary>
        /// Appointment identifier – used to resolve patient & doctor automatically
        /// </summary>
        [Required]
        public int AppointmentId { get; set; }

        /// <summary>
        /// Standard ARV regimen to apply
        /// </summary>
        [Required]
        public int RegimenId { get; set; }

        /// <summary>
        /// Treatment start date (defaults to today if not supplied)
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Expected end date (optional)
        /// </summary>
        public DateTime? ExpectedEndDate { get; set; }

        /// <summary>
        /// Actual dosage instructions (optional – free text)
        /// </summary>
        public string? ActualDosage { get; set; }

        /// <summary>
        /// Optional status when updating (InTreatment, Completed, Discontinued, ...)
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Reason for change or stop (if status changed)
        /// </summary>
        public string? ReasonForChangeOrStop { get; set; }

        /// <summary>
        /// Regimen adjustments (free text)
        /// </summary>
        public string? RegimenAdjustments { get; set; }
    }
} 