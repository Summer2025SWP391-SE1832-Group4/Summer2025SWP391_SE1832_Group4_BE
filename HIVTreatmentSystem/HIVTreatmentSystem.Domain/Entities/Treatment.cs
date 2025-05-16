using System;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Treatment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public required string TreatmentType { get; set; }
        public required string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required string Status { get; set; }
        public required string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual required Patient Patient { get; set; }
        public virtual required Doctor Doctor { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
} 