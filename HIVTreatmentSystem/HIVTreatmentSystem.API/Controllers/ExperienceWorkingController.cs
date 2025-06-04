using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Common;
using Microsoft.AspNetCore.Mvc;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.ExperienceWorking;
using System.Linq;
using System.Collections.Generic;

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
            if (result == null || !result.Any())
            {
                return Ok(new ApiResponse("No experience working records found.", new List<ExperienceWorkingDto>()));
            }
            return Ok(new ApiResponse("Success", result));
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
            if (result == null) return NotFound(new ApiResponse("Experience working record not found."));
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
            return CreatedAtAction(nameof(GetById), new { id = result.DoctorId }, result);
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
            return Ok(new ApiResponse("Experience working record updated successfully.", result));
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
            if (!success) return NotFound(new ApiResponse("Experience working record not found."));
            return Ok(new ApiResponse("Experience working record deleted successfully."));
        }

        /// <summary>
        /// Update all experience working records for a specific doctor (partial update).
        /// </summary>
        /// <param name="doctorId">The doctor's ID.</param>
        /// <param name="dto">The updated experience working data (only non-null fields will be updated).</param>
        /// <returns>The updated experience working records.</returns>
        [HttpPut("doctor/{doctorId}")]
        public async Task<IActionResult> UpdateByDoctorId(int doctorId, [FromBody] ExperienceWorkingDoctorDTO dto)
        {
            var result = await _service.UpdateByDoctorIdAsync(doctorId, dto);
            return Ok(new ApiResponse("Your work experience information has been updated successfully.", result));
        }
    }
} 