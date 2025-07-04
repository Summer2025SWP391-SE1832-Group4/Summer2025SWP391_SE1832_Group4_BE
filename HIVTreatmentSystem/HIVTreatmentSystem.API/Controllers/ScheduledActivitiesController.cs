using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    [Route("api/scheduled-activities")]
    [ApiController]
    public class ScheduledActivitiesController : ControllerBase
    {
        private readonly IScheduledActivityService _service;

        public ScheduledActivitiesController(IScheduledActivityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduledActivityResponse>>> GetAll(
            [FromQuery] int? patientId,
            [FromQuery] string? activityType
        )
        {
            var result = await _service.GetAllAsync(patientId, activityType);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduledActivityResponse>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ScheduledActivityResponse>> Create(
            ScheduledActivityRequest request
        )
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.ScheduledActivityId },
                result
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ScheduledActivityResponse>> Update(
            int id,
            ScheduledActivityRequest request
        )
        {
            var result = await _service.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
