using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.DoctorSchedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorScheduleController : ControllerBase
    {
        private readonly IDoctorScheduleService _doctorScheduleService;

        public DoctorScheduleController(IDoctorScheduleService doctorScheduleService)
        {
            _doctorScheduleService = doctorScheduleService;
        }

        /// <summary>
        /// Get all schedules for a specific doctor by ID
        /// </summary>
        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<DoctorScheduleDto>>> GetByDoctorId(int doctorId)
        {
            var schedules = await _doctorScheduleService.GetByDoctorIdAsync(doctorId);
            return Ok(schedules);
        }

        /// <summary>
        /// Get all schedules for a specific doctor by name
        /// </summary>
        [HttpGet("doctor/name/{doctorName}")]
        public async Task<ActionResult<IEnumerable<DoctorScheduleDto>>> GetByDoctorName(string doctorName)
        {
            var schedules = await _doctorScheduleService.GetByDoctorNameAsync(doctorName);
            return Ok(schedules);
        }

        /// <summary>
        /// Get a specific schedule by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorScheduleDto>> GetById(int id)
        {
            var schedule = await _doctorScheduleService.GetByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        /// <summary>
        /// Create a new doctor schedule
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<DoctorScheduleDto>> Create(DoctorScheduleDto dto)
        {
            var createdSchedule = await _doctorScheduleService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdSchedule.DoctorId }, createdSchedule);
        }

        /// <summary>
        /// Update an existing doctor schedule
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<DoctorScheduleDto>> Update(int id, DoctorScheduleDto dto)
        {
            var updatedSchedule = await _doctorScheduleService.UpdateAsync(id, dto);
            if (updatedSchedule == null)
            {
                return NotFound();
            }
            return Ok(updatedSchedule);
        }

        /// <summary>
        /// Delete a doctor schedule
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _doctorScheduleService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Create weekly schedules for all doctors
        /// </summary>
        [HttpPost("weekly")]
        public async Task<ActionResult<List<DoctorScheduleDto>>> CreateWeeklySchedule(CreateWeeklyScheduleDto dto)
        {
            var schedules = await _doctorScheduleService.CreateWeeklyScheduleAsync(dto);
            return Ok(schedules);
        }
    }
} 