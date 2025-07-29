using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HIVTreatmentSystem.API.Controllers;

[ApiController]
[Route("api/dashboard")]
// [Authorize] // Require authentication for all endpoints
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// Get total count of all patients
    /// </summary>
    /// <returns>Total patient count</returns>
    [HttpGet("statistics/total-patients")]
    public async Task<IActionResult> GetTotalPatients()
    {
        var totalCount = await _dashboardService.GetTotalPatientsCountAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Total patient count retrieved successfully",
            Data = new { TotalPatients = totalCount }
        });
    }

    /// <summary>
    /// Get test result statistics with positive/negative percentages
    /// </summary>
    /// <returns>Test result statistics with percentages</returns>
    [HttpGet("statistics/test-results-summary")]
    public async Task<IActionResult> GetTestResultsSummary()
    {
        var (totalTests, positiveCount, negativeCount) = await _dashboardService.GetTestResultsSummaryAsync();
        
        var data = new
        {
            TotalTests = totalTests,
            PositiveCount = positiveCount,
            NegativeCount = negativeCount,
            PositivePercentage = totalTests > 0 ? Math.Round((double)positiveCount / totalTests * 100, 2) : 0,
            NegativePercentage = totalTests > 0 ? Math.Round((double)negativeCount / totalTests * 100, 2) : 0
        };

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Test result statistics retrieved successfully",
            Data = data
        });
    }

    /// <summary>
    /// Get statistics for patients currently under treatment
    /// </summary>
    /// <returns>Current treatment statistics</returns>
    [HttpGet("statistics/current-treatment")]
    public async Task<IActionResult> GetCurrentTreatmentStatistics()
    {
        var result = await _dashboardService.GetPatientTreatmentStatusStatisticsAsync();
        var underTreatment = result.FirstOrDefault(x => x.TreatmentStatus == "InTreatment");
        
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Current treatment statistics retrieved successfully",
            Data = new
            {
                Count = underTreatment?.Count ?? 0,
                Percentage = underTreatment?.Percentage ?? 0
            }
        });
    }

    /// <summary>
    /// Get statistics for successfully treated patients
    /// </summary>
    /// <returns>Successful treatment statistics</returns>
    [HttpGet("statistics/successful-treatment")]
    public async Task<IActionResult> GetSuccessfulTreatmentStatistics()
    {
        var result = await _dashboardService.GetPatientTreatmentStatusStatisticsAsync();
        var successfulTreatment = result.FirstOrDefault(x => x.TreatmentStatus == "Completed");
        
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Successful treatment statistics retrieved successfully",
            Data = new
            {
                Count = successfulTreatment?.Count ?? 0,
                Percentage = successfulTreatment?.Percentage ?? 0
            }
        });
    }
}