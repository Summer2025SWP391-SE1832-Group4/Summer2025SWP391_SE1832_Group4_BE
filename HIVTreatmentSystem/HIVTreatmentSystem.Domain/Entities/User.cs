using System.ComponentModel.DataAnnotations;
using HIVTreatmentSystem.Domain.Common;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = null!;

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public UserRole Role { get; set; }

        public string? ProfileImageUrl { get; set; }

        public bool IsEmailVerified { get; set; } = false;

        [Required]
        public string EmailVerificationToken { get; set; } = null!;

        public string? PasswordResetToken { get; set; }

        public DateTime? PasswordResetTokenExpiry { get; set; }

        public DateTime? LastLogin { get; set; }

        // Navigation Properties
        public virtual ICollection<PatientRecord> PatientRecords { get; set; } =
            new List<PatientRecord>();
        public virtual ICollection<ExperienceWorking> ExperienceWorkings { get; set; } =
            new List<ExperienceWorking>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<BookingSlot> BookingSlots { get; set; } =
            new List<BookingSlot>();
        public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
    }
}
