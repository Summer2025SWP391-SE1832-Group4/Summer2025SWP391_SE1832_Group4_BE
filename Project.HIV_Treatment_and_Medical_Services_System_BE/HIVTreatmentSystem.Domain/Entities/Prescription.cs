using System;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Prescription
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid TreatmentId { get; set; }
        public required string MedicationName { get; set; }
        public required string Dosage { get; set; }
        public required string Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required string Instructions { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual required Patient Patient { get; set; }
        public virtual required Doctor Doctor { get; set; }
        public virtual required Treatment Treatment { get; set; }
    }
} 