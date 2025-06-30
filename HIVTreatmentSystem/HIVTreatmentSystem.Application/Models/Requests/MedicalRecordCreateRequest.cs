using System.ComponentModel.DataAnnotations;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    /// <summary>
    /// Request model used for creating a Medical Record for a Patient
    /// Mỗi Patient chỉ có một MedicalRecord duy nhất (1-to-1 relationship)
    /// </summary>
    public class MedicalRecordCreateRequest
    {
        [Required(ErrorMessage = "Doctor ID is required")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Consultation date is required")]
        public DateTime ConsultationDate { get; set; }

        public string? Symptoms { get; set; }

        public string? Diagnosis { get; set; }

        /// <summary>
        /// Trạng thái mang thai của bệnh nhân
        /// </summary>
        public PregnancyStatus PregnancyStatus { get; set; } = PregnancyStatus.Unknown;

        /// <summary>
        /// Phụ mang thai ở tuần thứ... (chỉ áp dụng khi PregnancyStatus = Pregnant)
        /// </summary>
        public int PregnancyWeek { get; set; }

        public string? DoctorNotes { get; set; }

        public string? NextSteps { get; set; }

        [MaxLength(255)]
        public string? UnderlyingDisease { get; set; }

        public string? DrugAllergyHistory { get; set; }
    }
} 