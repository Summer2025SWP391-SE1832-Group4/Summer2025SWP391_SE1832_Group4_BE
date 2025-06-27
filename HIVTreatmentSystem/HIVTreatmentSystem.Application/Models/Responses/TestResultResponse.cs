namespace HIVTreatmentSystem.Application.Models.Responses
{
    /// <summary>
    /// Response model for test result data
    /// </summary>
    public class TestResultResponse
    {
        /// <summary>
        /// Unique identifier for the test result
        /// </summary>
        public int TestResultId { get; set; }

        /// <summary>
        /// ID of the patient
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// ID of the associated medical record (if any)
        /// </summary>
        public int? MedicalRecordId { get; set; }

        /// <summary>
        /// Date when the test was performed
        /// </summary>
        public DateTime TestDate { get; set; }

        /// <summary>
        /// Type of test performed
        /// </summary>
        public string TestType { get; set; } = string.Empty;

        /// <summary>
        /// CD4 count result (if applicable)
        /// </summary>
        public int? CD4Count { get; set; }

        /// <summary>
        /// Unit for CD4 count measurement
        /// </summary>
        // public string CD4Unit { get; set; } = "cells/mm³";

        /// <summary>
        /// HIV viral load value (if applicable)
        /// </summary>
        public string? HivViralLoadValue { get; set; }

        /// <summary>
        /// Unit for HIV viral load measurement
        /// </summary>
        // public string HivViralLoadUnit { get; set; } = "copies/mL";

        /// <summary>
        /// Name of the laboratory where the test was performed
        /// </summary>
        public string? LabName { get; set; }

        /// <summary>
        /// URL to the attached test result file
        /// </summary>
        // public string? AttachedFileUrl { get; set; }

        /// <summary>
        /// Comments from the doctor about the test results
        /// </summary>
        public string? DoctorComments { get; set; }

        public string? TestResults { get; set; }


        /// <summary>
        /// Associated patient information
        /// </summary>
        public PatientResponse? Patient { get; set; }

        /// <summary>
        /// Associated medical record information (if any)
        /// </summary>
        public MedicalRecordResponse? MedicalRecord { get; set; }
    }
} 