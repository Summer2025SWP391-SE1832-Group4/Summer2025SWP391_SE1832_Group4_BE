using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    /// <summary>
    /// Request model for creating or updating a test result
    /// </summary>
    public class TestResultRequest
    {
        /// <summary>
        /// ID của cuộc hẹn liên quan (bắt buộc)
        /// </summary>
        [Required]
        public int AppointmentId { get; set; }

        /// <summary>
        /// Date when the test was performed
        /// </summary>
        [Required]
        public DateTime TestDate { get; set; }

        /// <summary>
        /// Type of test performed
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string TestType { get; set; } = string.Empty;

        /// <summary>
        /// CD4 count result (if applicable)
        /// </summary>
        public int? CD4Count { get; set; }

        /// <summary>
        /// Unit for CD4 count measurement
        /// </summary>
        [MaxLength(20)]
        public string CD4Unit { get; set; } = "cells/mm³";

        /// <summary>
        /// HIV viral load value (if applicable)
        /// </summary>
        [MaxLength(50)]
        public string? HivViralLoadValue { get; set; }

        /// <summary>
        /// Unit for HIV viral load measurement
        /// </summary>
        [MaxLength(20)]
        public string HivViralLoadUnit { get; set; } = "copies/mL";

        /// <summary>
        /// Name of the laboratory where the test was performed
        /// </summary>
        [MaxLength(100)]
        public string? LabName { get; set; }

        /// <summary>
        /// URL to the attached test result file
        /// </summary>
        // [MaxLength(255)]
        // public string? AttachedFileUrl { get; set; }
        /// <summary>
        /// Comments from the doctor about the test results
        /// </summary>
        public string? DoctorComments { get; set; }
    }
} 