namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Application.DTOs
{
    /// <summary>
    /// Login data transfer object.
    /// </summary>
    public class LoginDto
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
    }
}
