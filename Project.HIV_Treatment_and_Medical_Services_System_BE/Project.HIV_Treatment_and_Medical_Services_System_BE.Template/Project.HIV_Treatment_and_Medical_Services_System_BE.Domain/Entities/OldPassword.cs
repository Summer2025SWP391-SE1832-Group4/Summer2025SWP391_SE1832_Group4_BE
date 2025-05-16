namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities
{
    /// <summary>
    /// Old password entity.
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public class OldPassword : BaseEntity
    {
        /// <summary>
        /// Gets or sets the old password hash.
        /// </summary>
        /// <value>
        /// The old password hash.
        /// </value>
        public string? OldPasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public Guid? AccountId { get; set; }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public Account? Account { get; set; }
    }
}
