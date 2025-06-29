using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    /// <summary>
    /// Request model for creating or updating a Medical Record (based on TestResult)
    /// </summary>
    public class MedicalRecordRequest
    {
        /// <summary>
        /// ID of the associated test result
        /// </summary>
        [Required(ErrorMessage = "Test Result ID is required")]
        public int TestResultId { get; set; }

        /// <summary>
        /// ID of the doctor creating/updating the medical record
        /// </summary>
        [Required(ErrorMessage = "Doctor ID is required")]
        public int DoctorId { get; set; }

        /// <summary>
        /// Date of the consultation
        /// </summary>
        [Required(ErrorMessage = "Consultation date is required")]
        public DateTime ConsultationDate { get; set; }

        /// <summary>
        /// Patient's reported symptoms
        /// </summary>
        public string? Symptoms { get; set; }

        /// <summary>
        /// Doctor's diagnosis
        /// </summary>
        public string? Diagnosis { get; set; }

        /// <summary>
        /// Additional notes from the doctor
        /// </summary>
        public string? DoctorNotes { get; set; }

        /// <summary>
        /// Recommended next steps
        /// </summary>
        public string? NextSteps { get; set; }

        /// <summary>
        /// Any co-infection diseases
        /// </summary>
        [MaxLength(255, ErrorMessage = "Coinfection diseases cannot exceed 255 characters")]
        public string? CoinfectionDiseases { get; set; }

        /// <summary>
        /// Patient's drug allergy history
        /// </summary>
        public string? DrugAllergyHistory { get; set; }
    }
} 