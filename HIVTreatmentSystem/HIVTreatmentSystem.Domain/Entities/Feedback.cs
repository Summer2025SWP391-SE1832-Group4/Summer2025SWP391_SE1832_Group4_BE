using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Common;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Feedback : BaseEntity
    {
        public int UserId { get; set; }
        public int? BookingId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        [StringLength(100)]
        public string FeedbackType { get; set; } // Service, Doctor, System

        public bool IsAnonymous { get; set; } = false;

        public bool IsApproved { get; set; } = false;

        public DateTime? ApprovedAt { get; set; }

        public int? ApprovedBy { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }
    }
}
