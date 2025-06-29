using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    /// <summary>
    /// Request model used for creating a Medical Record based on TestResult (PatientId được tự động lấy từ TestResult)
    /// </summary>
    public class MedicalRecordCreateRequest
    {
        [Required(ErrorMessage = "Test Result ID is required")]
        public int TestResultId { get; set; }

        [Required(ErrorMessage = "Doctor ID is required")]
        public int DoctorId { get; set; }

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