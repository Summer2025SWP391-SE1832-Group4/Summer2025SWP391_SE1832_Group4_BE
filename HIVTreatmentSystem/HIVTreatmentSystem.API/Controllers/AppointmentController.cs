using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
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
        [FromQuery] AppointmentTypeEnum? appointmentType,
        [FromQuery] AppointmentStatus? status,
        [FromQuery] AppointmentServiceEnum? appointmentService,
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        [FromQuery] bool isDescending = false,
        [FromQuery] string? sortBy = "",
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
        {
            var result = await _appointmentService.GetAllAppointmentsAsync(
                doctorName,
                patientName,
                appointmentType,
                status,
                appointmentService,
                startDate,
                endDate,
                isDescending,
                sortBy,
                pageIndex,
                pageSize);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
                return NotFound(new ApiResponse("Error: Appointment not found"));

            return Ok(new ApiResponse("Appointment retrieved successfully", appointment));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentRequest request)
        {
            var response = await _appointmentService.CreateAppointmentAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentUpdateRequest request)
        {

                var result = await _appointmentService.UpdateAppointmentAsync(id, request);

                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await _appointmentService.DeleteAppointmentAsync(id);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }




    }

}
