using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.WebAPI.Controllers
{
    /// <summary>
    /// Controller Base.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    public class ControllerBase(IMediator mediator, IMapper mapper) : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        /// <summary>
        /// The mediator
        /// </summary>
        public readonly IMediator Mediator = mediator;

        /// <summary>
        /// The mapper
        /// </summary>
        public readonly IMapper Mapper = mapper;
    }
}
