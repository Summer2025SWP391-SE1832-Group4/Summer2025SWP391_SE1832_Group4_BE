using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Patient
    {
        public int PatientId { get; set; } // Same as UserId

        [MaxLength(50)]
        public string? PatientCodeAtFacility { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender? Gender { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        public DateTime? HivDiagnosisDate { get; set; }

        public string? ConsentInformation { get; set; }

        [MaxLength(50)]
        public string? AnonymousIdentifier { get; set; }

        public string? AdditionalNotes { get; set; }

        // Navigation properties
        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<Appointment> Appointments { get; set; } =
            new List<Appointment>();
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } =
            new List<MedicalRecord>();
        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
        public virtual ICollection<PatientTreatment> Treatments { get; set; } =
            new List<PatientTreatment>();
        public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
    }
}
