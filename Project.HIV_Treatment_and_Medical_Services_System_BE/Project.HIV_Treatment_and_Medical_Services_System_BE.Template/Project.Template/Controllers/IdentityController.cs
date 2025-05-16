using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Application.DTOs;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Application.Usecases.Login;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.WebAPI.Controllers
{
    /// <summary>
    /// Identity controller.
    /// </summary>
    /// <seealso cref="Project.Template.WebAPI.Controllers.ControllerBase" />
    [Route("api/[controller]")]
    public class IdentityController(IMediator mediator, IMapper mapper) : ControllerBase(mediator, mapper)
    {
        /// <summary>
        /// Logins the specified login dto.
        /// </summary>
        /// <param name="loginDto">The login dto.</param>
        /// <returns>Ok and access token</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var loginCommand = this.Mapper.Map<LoginCommand>(loginDto);

            var result = await this.Mediator.Send(loginCommand);

            return Ok(result);
        }
    }
}
