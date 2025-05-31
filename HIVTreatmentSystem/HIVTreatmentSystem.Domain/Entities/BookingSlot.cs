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
    public class BookingSlot : BaseEntity
    {
        public int? DoctorId { get; set; }

        public DateTime SlotDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public bool IsAvailable { get; set; } = true;

        public int MaxBookings { get; set; } = 1;

        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public bool IsRecurring { get; set; } = false;

        [StringLength(50)]
        public string RecurringPattern { get; set; }

        // Navigation Properties
        [ForeignKey("DoctorId")]
        public virtual User Doctor { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
