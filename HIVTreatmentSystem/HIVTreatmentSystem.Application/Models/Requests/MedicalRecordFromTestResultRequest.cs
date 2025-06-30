using System.ComponentModel.DataAnnotations;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    /// <summary>
    /// Request model for creating a Medical Record based on Test Result
    /// Bác sĩ chỉ cần nhập các thông tin cơ bản, test data đã có trong TestResult
    /// </summary>
    public class MedicalRecordFromTestResultRequest
    {
        /// <summary>
        /// ID của test result chính mà medical record này dựa vào
        /// </summary>
        [Required(ErrorMessage = "Test Result ID is required")]
        public int TestResultId { get; set; }

        /// <summary>
        /// ID của doctor tạo medical record
        /// </summary>
        [Required(ErrorMessage = "Doctor ID is required")]
        public int DoctorId { get; set; }

        /// <summary>
        /// Ngày tạo hồ sơ bệnh án
        /// </summary>
        [Required(ErrorMessage = "Consultation date is required")]
        public DateTime ConsultationDate { get; set; }

        /// <summary>
        /// Triệu chứng mà bệnh nhân báo cáo
        /// </summary>
        public string? Symptoms { get; set; }

        /// <summary>
        /// Chẩn đoán của bác sĩ dựa trên test result và triệu chứng
        /// </summary>
        public string? Diagnosis { get; set; }

        /// <summary>
        /// Trạng thái mang thai của bệnh nhân
        /// </summary>
        public PregnancyStatus PregnancyStatus { get; set; } = PregnancyStatus.Unknown;

        /// <summary>
        /// Phụ mang thai ở tuần thứ... (chỉ áp dụng khi PregnancyStatus = Pregnant)
        /// </summary>
        public int PregnancyWeek { get; set; }

        /// <summary>
        /// Ghi chú của bác sĩ về tình trạng bệnh nhân
        /// </summary>
        public string? DoctorNotes { get; set; }

        /// <summary>
        /// Các bước điều trị tiếp theo
        /// </summary>
        public string? NextSteps { get; set; }

        /// <summary>
        /// Các bệnh đồng nhiễm khác
        /// </summary>
        [MaxLength(255, ErrorMessage = "Coinfection diseases cannot exceed 255 characters")]
        public string? UnderlyingDisease { get; set; }

        /// <summary>
        /// Lịch sử dị ứng thuốc của bệnh nhân
        /// </summary>
        public string? DrugAllergyHistory { get; set; }
    }
} 