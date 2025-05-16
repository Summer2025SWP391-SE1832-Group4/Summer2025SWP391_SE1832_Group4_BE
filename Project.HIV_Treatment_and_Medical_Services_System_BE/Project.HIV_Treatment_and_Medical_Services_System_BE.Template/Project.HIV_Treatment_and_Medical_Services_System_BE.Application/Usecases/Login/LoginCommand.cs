using MediatR;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Application.DTOs;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Application.Usecases.Login
{
    /// <summary>
    /// Login command.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;Project.HIV_Treatment_and_Medical_Services_System_BE.Application.DTOs.LoginDto&gt;" />
    public class LoginCommand : IRequest<UserSessionDto>
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
