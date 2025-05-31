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
    public class Booking : BaseEntity
    {
        public int PatientId { get; set; }
        public int? DoctorId { get; set; }
        public int? SlotId { get; set; }

        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }

        [StringLength(500)]
        public string Purpose { get; set; }

        public BookingStatus Status { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        public bool IsOnline { get; set; } = false;
        public string MeetingUrl { get; set; }

        [StringLength(1000)]
        public string CancellationReason { get; set; }

        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        // Navigation Properties
        [ForeignKey("PatientId")]
        public virtual PatientRecord Patient { get; set; }

        [ForeignKey("DoctorId")]
        public virtual User Doctor { get; set; }

        [ForeignKey("SlotId")]
        public virtual BookingSlot Slot { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
