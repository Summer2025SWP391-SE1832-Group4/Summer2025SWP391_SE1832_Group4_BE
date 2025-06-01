using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Reminder
    {
        public int ReminderId { get; set; }

        public int PatientId { get; set; }

        public ReminderType ReminderType { get; set; }

        public int? PatientTreatmentId { get; set; }

        public int? AppointmentId { get; set; }

        public DateTime ReminderDateTime { get; set; }

        [MaxLength(50)]
        public string? Frequency { get; set; }

        public NotificationMethod? NotificationMethod { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        public string? ReminderContent { get; set; }

        // Navigation properties
        public virtual Patient Patient { get; set; } = null!;
        public virtual PatientTreatment? PatientTreatment { get; set; }
        public virtual Appointment? Appointment { get; set; }
    }
}
