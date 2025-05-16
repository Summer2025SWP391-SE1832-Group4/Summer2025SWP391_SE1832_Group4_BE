namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities
{
    /// <summary>
    /// User session.
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public class UserSession : BaseEntity
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the referesh token.
        /// </summary>
        /// <value>
        /// The referesh token.
        /// </value>
        public string? RefereshToken { get; set; }

        /// <summary>
        /// Gets or sets the expired on.
        /// </summary>
        /// <value>
        /// The expired on.
        /// </value>
        public DateTimeOffset? ExpiredOn { get; set; }

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
    }
}
