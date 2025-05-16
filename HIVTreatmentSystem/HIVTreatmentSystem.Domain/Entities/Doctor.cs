using System;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Doctor
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Specialization { get; set; }
        public required string LicenseNumber { get; set; }
        public required string ContactNumber { get; set; }
        public required string Email { get; set; }
        public required string Department { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
} 