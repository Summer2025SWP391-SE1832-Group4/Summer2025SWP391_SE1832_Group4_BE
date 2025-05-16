using Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Enum;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Application.DTOs
{
    /// <summary>
    /// Account data transfer object.
    /// </summary>
    public class AccountDto
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public AccountStatus Status { get; set; }
    }
}
