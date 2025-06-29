using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    /// <summary>
    /// Request model for creating or updating a Medical Record
    /// Mỗi Patient chỉ có một MedicalRecord duy nhất (1-to-1 relationship)
    /// </summary>
    public class MedicalRecordRequest
    {
        /// <summary>
        /// ID of the patient (required for 1-to-1 relationship)
        /// </summary>
        [Required(ErrorMessage = "Patient ID is required")]
        public int PatientId { get; set; }

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