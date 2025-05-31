using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Common;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Reminder : BaseEntity
    {
        public int UserId { get; set; }
        public int? PatientId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Message { get; set; }

        public DateTime ReminderDate { get; set; }

        public ReminderType Type { get; set; }

        public bool IsRecurring { get; set; } = false;

        [StringLength(50)]
        public string RecurringPattern { get; set; } // Daily, Weekly, Monthly

        public bool IsSent { get; set; } = false;

        public bool IsEmailReminder { get; set; } = true;

        public bool IsRead { get; set; } = false;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("PatientId")]
        public virtual PatientRecord Patient { get; set; }
    }
}
