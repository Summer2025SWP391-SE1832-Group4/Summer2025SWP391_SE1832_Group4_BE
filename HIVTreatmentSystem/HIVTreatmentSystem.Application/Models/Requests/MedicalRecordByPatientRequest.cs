using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    /// <summary>
    /// Request model for creating a Medical Record based on Patient ID
    /// </summary>
    public class MedicalRecordByPatientRequest
    {
        /// <summary>
        /// ID of the patient
        /// </summary>
        [Required(ErrorMessage = "Patient ID is required")]
        public int PatientId { get; set; }

        /// <summary>
        /// ID of the doctor creating the medical record
        /// </summary>
        [Required(ErrorMessage = "Doctor ID is required")]
        public int DoctorId { get; set; }

        /// <summary>
        /// Optional appointment ID (if related to specific appointment)
        /// </summary>
        public int? AppointmentId { get; set; }

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