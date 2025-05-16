using System;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Patient
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public required string ContactNumber { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string MedicalRecordNumber { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public required string HIVStatus { get; set; }
        public required string CurrentStage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
} 