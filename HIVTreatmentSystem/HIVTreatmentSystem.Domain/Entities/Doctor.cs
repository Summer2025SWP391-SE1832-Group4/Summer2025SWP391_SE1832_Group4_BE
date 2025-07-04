
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HIVTreatmentSystem.Domain.Entities
{
    public class Doctor
    {
        public int DoctorId { get; set; } // Same as UserId

        [MaxLength(100)]
        public string? Specialty { get; set; }

        public string? Qualifications { get; set; }

        public int? YearsOfExperience { get; set; }

        [MaxLength(500)]
        public string? ShortDescription { get; set; }

        // Navigation properties
        // Add this line for foreign key to Account
        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<Appointment> Appointments { get; set; } =
            new List<Appointment>();
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } =
            new List<MedicalRecord>();
        public virtual ICollection<PatientTreatment> PrescribedTreatments { get; set; } =
            new List<PatientTreatment>();
        public virtual ICollection<ExperienceWorking> ExperienceWorkings { get; set; } =
            new List<ExperienceWorking>();
        public virtual ICollection<Certificate> Certificates { get; set; } =             
            new List<Certificate>();
    }
}
