using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class MedicalRecord
    {
        public int MedicalRecordId { get; set; }

        /// <summary>
        /// Mỗi Patient chỉ có một MedicalRecord duy nhất (1-to-1 relationship)
        /// </summary>
        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        /// <summary>
        /// Ngày tạo hồ sơ bệnh án
        /// </summary>
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
        /// Phụ mang thai ở tuần thứ....
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
        [MaxLength(255)]
        public string? UnderlyingDisease { get; set; }

        /// <summary>
        /// Lịch sử dị ứng thuốc của bệnh nhân
        /// </summary>
        public string? DrugAllergyHistory { get; set; }

        // Navigation properties
        /// <summary>
        /// Patient có 1-to-1 relationship với MedicalRecord
        /// </summary>
        public virtual Patient Patient { get; set; } = null!;
        
        /// <summary>
        /// Bác sĩ đã tạo/cập nhật hồ sơ này
        /// </summary>
        public virtual Doctor Doctor { get; set; } = null!;
        
        /// <summary>
        /// Tất cả các test result thuộc về Patient này
        /// (TestResult sẽ link tới MedicalRecord thông qua MedicalRecordId)
        /// </summary>
        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
