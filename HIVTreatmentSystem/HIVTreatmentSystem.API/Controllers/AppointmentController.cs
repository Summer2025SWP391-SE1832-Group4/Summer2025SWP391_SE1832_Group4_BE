using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Services.DoctorService;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;

        public AppointmentController(IAppointmentService appointmentService, IDoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
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
            [FromQuery] int pageSize = 10
        )
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
                pageSize
            );

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

        [HttpPost("doctor")]
        public async Task<IActionResult> CreateAppointmentForDoctor([FromBody] AppointmentByDoctorRequest request)
        {
            var response = await _appointmentService.CreateAppointmentForDoctorAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(
            int id,
            [FromBody] AppointmentUpdateRequest request
        )
        {
            var result = await _appointmentService.UpdateAppointmentAsync(id, request);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPut("{id}/schedule")]
        public async Task<IActionResult> ScheduleAppointment(int id)
        {
            var result = await _appointmentService.SetStatusScheduledAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut("{id}/checkin")]
        public async Task<IActionResult> CheckInAppointment(int id)
        {
            var result = await _appointmentService.SetStatusCheckedInAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteAppointment(int id)
        {
            var result = await _appointmentService.SetStatusCompletedAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
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

        [HttpGet("by-account")]
        public async Task<IActionResult> GetAppointmentsByAccountId()
        {
            var appointments = await _appointmentService.GetAppointmentsByTokenAsync();

            return Ok(new ApiResponse("Appointments retrieved successfully", appointments));
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodaysAppointments([FromQuery] string? phoneNumber)
        {
            var list = await _appointmentService.GetTodaysAppointmentsAsync(phoneNumber);
            return Ok(new ApiResponse("Today's appointments retrieved successfully", list));
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableDoctors([FromQuery] DateOnly date, [FromQuery] TimeOnly time, [FromQuery] AppointmentTypeEnum specialty)
        {
            var availableDoctors = await _doctorService.GetAvailableDoctorsAsync(date, time, specialty);
            return Ok(availableDoctors);
        }
    }
}
