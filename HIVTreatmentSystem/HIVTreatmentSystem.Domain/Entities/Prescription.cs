using System;
using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Domain.Entities
{
    /// <summary>
    /// Thực thể đơn thuốc cho bệnh nhân.
    /// </summary>
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int PatientTreatmentId { get; set; }
        [Required]
        [MaxLength(100)]
        public string MedicationName { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string Dosage { get; set; } = string.Empty;
        public DateTime PrescribedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        // Navigation properties
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual PatientTreatment PatientTreatment { get; set; }
    }
} 