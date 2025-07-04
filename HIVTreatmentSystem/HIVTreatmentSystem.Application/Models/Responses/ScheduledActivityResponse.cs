
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class ScheduledActivityResponse
    {
        public int ScheduledActivityId { get; set; }
        public int PatientId { get; set; }
        public int? CreatedByStaffId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public ActivityType ActivityType { get; set; }
        public string? Description { get; set; }
        public ActivityStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
