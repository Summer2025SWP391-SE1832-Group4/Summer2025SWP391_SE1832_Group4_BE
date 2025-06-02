using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.ExperienceWorking;

namespace HIVTreatmentSystem.API.Controllers
{
    /// <summary>
    /// Manages doctor's experience working records.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceWorkingController : ControllerBase
    {
        private readonly IExperienceWorkingService _service;

        public ExperienceWorkingController(IExperienceWorkingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all experience working records for a specific doctor.
        /// </summary>
        /// <param name="doctorId">The doctor's ID.</param>
        /// <returns>List of experience working records.</returns>
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctorId(int doctorId)
        {
            var result = await _service.GetByDoctorIdAsync(doctorId);
            return Ok(result);
        }

        /// <summary>
        /// Get a specific experience working record by its ID.
        /// </summary>
        /// <param name="id">The experience working record ID.</param>
        /// <returns>The experience working record details.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Create a new experience working record for a doctor.
        /// </summary>
        /// <param name="dto">The experience working data.</param>
        /// <returns>The created experience working record.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExperienceWorkingDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing experience working record.
        /// </summary>
        /// <param name="id">The experience working record ID.</param>
        /// <param name="dto">The updated experience working data.</param>
        /// <returns>The updated experience working record.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExperienceWorkingDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Delete an experience working record by its ID.
        /// </summary>
        /// <param name="id">The experience working record ID.</param>
        /// <returns>No content if deleted successfully.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
} 