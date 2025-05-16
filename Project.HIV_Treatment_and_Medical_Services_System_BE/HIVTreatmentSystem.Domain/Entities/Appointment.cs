using System;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public required string AppointmentType { get; set; }
        public required string Status { get; set; }
        public required string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual required Patient Patient { get; set; }
        public virtual required Doctor Doctor { get; set; }
    }
} 