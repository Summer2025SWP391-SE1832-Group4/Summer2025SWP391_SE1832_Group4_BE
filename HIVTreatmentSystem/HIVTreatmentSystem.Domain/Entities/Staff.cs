
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HIVTreatmentSystem.Domain.Entities
{
    public class Staff
    {
        public int StaffId { get; set; } // Same as UserId

        [MaxLength(100)]
        public string? Position { get; set; }

        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<ScheduledActivity> CreatedSchedules { get; set; } =
            new List<ScheduledActivity>();
    }
}
