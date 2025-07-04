
using System.ComponentModel.DataAnnotations;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class ScheduledActivityRequest
    {
        [Required]
        public int PatientId { get; set; }

        public int? CreatedByStaffId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        public ActivityType ActivityType { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public ActivityStatus Status { get; set; }
    }
}
