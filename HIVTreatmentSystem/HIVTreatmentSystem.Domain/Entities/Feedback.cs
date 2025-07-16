
using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } 

        [MaxLength(1000)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}