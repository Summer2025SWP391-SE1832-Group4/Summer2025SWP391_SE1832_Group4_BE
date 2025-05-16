namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities
{
    /// <summary>
    /// User entity.
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public class User : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public Account? Account { get; set; }

        /// <summary>
        /// Gets or sets the user sessions.
        /// </summary>
        /// <value>
        /// The user sessions.
        /// </value>
        public List<UserSession>? UserSessions { get; set; }
    }
}
