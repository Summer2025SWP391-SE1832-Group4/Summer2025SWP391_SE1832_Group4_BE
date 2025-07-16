using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    [Route("api/adverseeffectreports")]
    [ApiController]
    public class AdverseEffectReportController : ControllerBase
    {

        private readonly IAdverseEffectReportService _adverseEffectReportService;

        public AdverseEffectReportController(IAdverseEffectReportService adverseEffectReportService)
        {
            _adverseEffectReportService = adverseEffectReportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdverseEffectSeveritiesAsync(
            [FromQuery] int? accountId,
            [FromQuery] DateOnly? dateOccurred,
            [FromQuery] AdverseEffectSeverityEnum? severity,
            [FromQuery] AdverseEffectReportStatusEnum? status,
            [FromQuery] DateOnly? startDate,
            [FromQuery] DateOnly? endDate,
            [FromQuery] bool isDescending = false,
            [FromQuery] string? sortBy = "dateOccurred",
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10
            )
        {
            var result = await _adverseEffectReportService.GetAdverseEffectReportsAsync(
                accountId,
                dateOccurred,
                severity,
                status,
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
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _adverseEffectReportService.GetByIdAsync(id);
            if (appointment == null)
                return NotFound(new ApiResponse("Error: Adverse Effect Report not found"));

            return Ok(new ApiResponse("Adverse Effect Report retrieved successfully", appointment));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdverseEffectReportRequest request)
        {
            var response = await _adverseEffectReportService.CreateAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await _adverseEffectReportService.DeleteAsync(id);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AdverseEffectReportUpdateRequest request)
        {
            var result = await _adverseEffectReportService.UpdateAsync(id, request);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
