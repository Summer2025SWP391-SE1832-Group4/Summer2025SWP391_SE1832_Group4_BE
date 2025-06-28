using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }

        [MaxLength(50)]
        public AppointmentTypeEnum AppointmentType { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.PendingConfirmation;

        public AppointmentServiceEnum? AppointmentService { get; set; }

        public string? AppointmentNotes { get; set; }


        public int? CreatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Patient Patient { get; set; } = null!;
        public virtual Doctor? Doctor { get; set; }
        public virtual Account? CreatedByUser { get; set; }
        public virtual MedicalRecord? MedicalRecord { get; set; }
        public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
