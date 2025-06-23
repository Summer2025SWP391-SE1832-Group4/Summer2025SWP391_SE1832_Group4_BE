using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    /// <summary>
    /// Request model used ONLY for creating a Medical Record (PatientId & DoctorId được tự động lấy từ Appointment)
    /// </summary>
    public class MedicalRecordCreateRequest
    {
        [Required(ErrorMessage = "Appointment ID is required")]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Consultation date is required")]
        public DateTime ConsultationDate { get; set; }

        public string? Symptoms { get; set; }

        public string? Diagnosis { get; set; }

        public string? DoctorNotes { get; set; }

        public string? NextSteps { get; set; }

        [MaxLength(255)]
        public string? CoinfectionDiseases { get; set; }

        public string? DrugAllergyHistory { get; set; }
    }
} 