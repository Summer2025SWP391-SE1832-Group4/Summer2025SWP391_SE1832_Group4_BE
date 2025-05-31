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
    public class PatientRecord : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string PatientCode { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime DiagnosisDate { get; set; }

        public PatientStatus Status { get; set; }

        [StringLength(100)]
        public string EmergencyContact { get; set; }

        [StringLength(15)]
        public string EmergencyPhone { get; set; }

        public int? ARVStandardId { get; set; }
        public int? DoctorId { get; set; }

        // Navigation Properties
        [ForeignKey("ARVStandardId")]
        public virtual ARVStandard ARVStandard { get; set; }

        [ForeignKey("DoctorId")]
        public virtual User Doctor { get; set; }

        public virtual ICollection<ExperienceWorking> ExperienceWorkings { get; set; } =
            new List<ExperienceWorking>();
        public virtual ICollection<CD4Test> CD4Tests { get; set; } = new List<CD4Test>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Certificate> Certificates { get; set; } =
            new List<Certificate>();
        public virtual ICollection<Reminder> PatientReminders { get; set; } = new List<Reminder>();
    }
}
