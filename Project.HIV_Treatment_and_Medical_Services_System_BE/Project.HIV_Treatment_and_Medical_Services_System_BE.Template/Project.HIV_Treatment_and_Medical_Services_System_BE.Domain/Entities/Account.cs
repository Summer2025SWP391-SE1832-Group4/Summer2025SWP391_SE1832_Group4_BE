using Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Enum;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities
{
    /// <summary>
    /// Account entity
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public class Account : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public AccountStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User? User { get; set; }

        /// <summary>
        /// Gets or sets the old passwords.
        /// </summary>
        /// <value>
        /// The old passwords.
        /// </value>
        public List<OldPassword>? OldPasswords { get; set; }
    }
}
