using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class MedicalRecord
    {
        public int MedicalRecordId { get; set; }

        /// <summary>
        /// TestResult chính mà MedicalRecord này dựa vào (bắt buộc)
        /// </summary>
        public int TestResultId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        /// <summary>
        /// Ngày tạo hồ sơ bệnh án (có thể khác với ngày test)
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
        public string? CoinfectionDiseases { get; set; }

        /// <summary>
        /// Lịch sử dị ứng thuốc của bệnh nhân
        /// </summary>
        public string? DrugAllergyHistory { get; set; }

        // Navigation properties
        /// <summary>
        /// TestResult chính mà hồ sơ này dựa vào
        /// </summary>
        public virtual TestResult TestResult { get; set; } = null!;
        
        public virtual Patient Patient { get; set; } = null!;
        public virtual Doctor Doctor { get; set; } = null!;
        
        /// <summary>
        /// Các test result bổ sung khác (ngoài test result chính)
        /// </summary>
        public virtual ICollection<TestResult> AdditionalTestResults { get; set; } = new List<TestResult>();
    }
}
