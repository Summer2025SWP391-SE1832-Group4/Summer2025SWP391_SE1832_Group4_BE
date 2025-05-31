using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Common;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class ExperienceWorking : BaseEntity
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public DateTime WorkingDate { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        [StringLength(1000)]
        public string Treatment { get; set; }

        [StringLength(500)]
        public string Prescription { get; set; }

        [StringLength(200)]
        public string Symptoms { get; set; }

        [StringLength(200)]
        public string Diagnosis { get; set; }

        public DateTime? NextAppointment { get; set; }

        // Navigation Properties
        [ForeignKey("PatientId")]
        public virtual PatientRecord Patient { get; set; }

        [ForeignKey("DoctorId")]
        public virtual User Doctor { get; set; }
    }

    // Certificate Management
    public class Certificate : BaseEntity
    {
        public int PatientId { get; set; }

        [Required]
        public CertificateType CertificateType { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string DocumentUrl { get; set; }

        [StringLength(100)]
        public string IssuedBy { get; set; }

        public bool IsValid { get; set; } = true;

        // Navigation Properties
        [ForeignKey("PatientId")]
        public virtual PatientRecord Patient { get; set; }
    }
}
