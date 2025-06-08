using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase

    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointmentsAsync(
        [FromQuery] string? doctorName,
        [FromQuery] string? patientName,
        [FromQuery] string? appointmentType,
        [FromQuery] AppointmentStatus? status,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] bool isDescending = false,
        [FromQuery] string? sortBy = "appointmentDateTime",
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
        {
            var result = await _appointmentService.GetAllAppointmentsAsync(
                doctorName,
                patientName,
                appointmentType,
                status,
                startDate,
                endDate,
                isDescending,
                sortBy,
                pageIndex,
                pageSize);

            return Ok(result);
        }
    }
}
