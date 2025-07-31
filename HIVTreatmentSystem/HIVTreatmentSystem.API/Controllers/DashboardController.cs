using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? entity,
        [FromQuery] string? groupBy,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to
    )
    {
        var (items, total) = await _service.GetAllAsync(entity, groupBy, from, to);
        return Ok(
            new ApiResponse
            {
                Success = true,
                Message = "Dashboard statistics retrieved successfully",
                Data = new { Items = items, TotalCount = total },
            }
        );
    }

    [HttpGet("statistics/test-result-summary")]
    public async Task<IActionResult> GetTestResultSummary()
    {
        var data = await _service.GetTestResultSummaryAsync();
        return Ok(
            new ApiResponse
            {
                Success = true,
                Message = "Test result statistics retrieved successfully",
                Data = data,
            }
        );
    }

    [HttpGet("treatments")]
    public async Task<IActionResult> GetPatientTreatments(
        [FromQuery] string? statusFilter,
        [FromQuery] string? sortBy,
        [FromQuery] bool sortDesc = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20
    )
    {
        var (items, total) = await _service.GetPatientTreatmentsAsync(
            statusFilter,
            sortBy,
            sortDesc,
            pageNumber,
            pageSize
        );

        return Ok(
            new ApiResponse
            {
                Success = true,
                Message = "Patient treatments retrieved successfully",
                Data = new { Items = items, TotalCount = total },
            }
        );
    }

    [HttpGet("statistics/treatment-status-count")]
    public async Task<IActionResult> GetTreatmentStatusCount()
    {
        var data = await _service.GetTreatmentStatusCountAsync();
        return Ok(
            new ApiResponse
            {
                Success = true,
                Message = "Treatment status counts retrieved successfully",
                Data = data,
            }
        );
    }
}
