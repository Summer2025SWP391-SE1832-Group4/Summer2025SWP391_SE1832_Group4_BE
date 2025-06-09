// [DOCTOR SCHEDULE API] - Controller for managing doctor schedules
// MERGE: Combined changes from dev and main branches
// - Added audit logging for all operations
// - Added API response wrapping with ApiResponse
// - Added model validation
// - Added error handling with proper status codes
// - Added XML documentation for all endpoints
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.DoctorSchedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.DTOs;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.API.Controllers
{
    /// <summary>
    /// API Controller for managing doctor schedules.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/doctorSchedule")]
    public class DoctorScheduleController : ControllerBase
    {
        // [DOCTOR SCHEDULE API] - Controller dependencies
        private readonly IDoctorScheduleService _service;
        private readonly ISystemAuditLogService _auditService;
        private readonly IMonthlyScheduleService _monthlyScheduleService;

        // [DOCTOR SCHEDULE API] - Constructor with all required services
        public DoctorScheduleController(
            IDoctorScheduleService service, 
            ISystemAuditLogService auditService, 
            IMonthlyScheduleService monthlyScheduleService)
        {
            _service = service;
            _auditService = auditService;
            _monthlyScheduleService = monthlyScheduleService;
        }

        // [DOCTOR SCHEDULE API] - Helper method for audit logging
        private async Task LogAction(string action, string? entityId = null, string? details = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _auditService.LogAsync(new SystemAuditLog
            {
                AccountId = int.TryParse(userId, out var id) ? id : null,
                Username = username,
                RoleAtTimeOfAction = role,
                Action = action,
                AffectedEntity = "DoctorSchedule",
                AffectedEntityId = entityId,
                IpAddress = ip,
                ActionDetails = details
            });
        }

        /// <summary>
        /// [DOCTOR SCHEDULE API] - Get all schedules for a specific doctor by ID
        /// </summary>
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctorId(int doctorId)
        {
            var result = await _service.GetByDoctorIdAsync(doctorId);
            if (result == null || !result.Any())
            {
                return Ok(new ApiResponse("No doctor schedules found.", new List<DoctorScheduleDto>()));
            }
            return Ok(new ApiResponse("Success", result));
        }

        /// <summary>
        /// [DOCTOR SCHEDULE API] - Get all schedules for a specific doctor by name
        /// </summary>
        [HttpGet("doctor/by-name/{doctorName}")]
        public async Task<IActionResult> GetByDoctorName(string doctorName)
        {
            var result = await _service.GetByDoctorNameAsync(doctorName);
            if (result == null || !result.Any())
            {
                return Ok(new ApiResponse("No doctor schedules found.", new List<DoctorScheduleDto>()));
            }
            return Ok(new ApiResponse("Success", result));
        }

        /// <summary>
        /// [DOCTOR SCHEDULE API] - Get a specific schedule by its ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new ApiResponse("Doctor schedule not found."));
            return Ok(new ApiResponse("Success", result));
        }

        /// <summary>
        /// [DOCTOR SCHEDULE API] - Create a new doctor schedule
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DoctorScheduleDto dto)
        {
            var result = await _service.CreateAsync(dto);
            await LogAction("Created doctor schedule", null, System.Text.Json.JsonSerializer.Serialize(dto));
            return Ok(new ApiResponse("Doctor schedule created successfully.", result));
        }

        /// <summary>
        /// [DOCTOR SCHEDULE API] - Update an existing doctor schedule
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DoctorScheduleDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            await LogAction("Updated doctor schedule", id.ToString(), System.Text.Json.JsonSerializer.Serialize(dto));
            if (result == null) return NotFound();
            return Ok(new ApiResponse("Doctor schedule updated successfully.", result));
        }

        /// <summary>
        /// [DOCTOR SCHEDULE API] - Delete a doctor schedule by its ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            await LogAction("Deleted doctor schedule", id.ToString());
            if (!success) return NotFound();
            return Ok(new ApiResponse("Doctor schedule deleted successfully."));
        }
        
        /// <summary>
        /// [DOCTOR SCHEDULE API] - Create weekly schedules for a doctor schedule
        /// </summary>
        [HttpPost("weekly")]
        public async Task<IActionResult> CreateWeeklySchedule([FromBody] CreateWeeklyScheduleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var schedules = await _service.CreateWeeklyScheduleAsync(dto);
            await LogAction("Created weekly schedules for all doctors", null, System.Text.Json.JsonSerializer.Serialize(dto));
            return Ok(new ApiResponse("Weekly schedules created successfully.", schedules));
        }
    }
} 