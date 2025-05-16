using MediatR;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Application.DTOs;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Application.Usecases.Login
{
    /// <summary>
    /// Login command handler.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;Project.HIV_Treatment_and_Medical_Services_System_BE.Application.Services.Login.LoginCommand, Project.HIV_Treatment_and_Medical_Services_System_BE.Application.DTOs.UserSessionDto&gt;" />
    /// <seealso cref="MediatR.IRequestHandler&lt;Project.HIV_Treatment_and_Medical_Services_System_BE.Application.Services.Login.LoginCommand, Project.HIV_Treatment_and_Medical_Services_System_BE.Application.DTOs.LoginDto&gt;" />
    public class LoginCommandHandler : IRequestHandler<LoginCommand, UserSessionDto>
    {
        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<UserSessionDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
