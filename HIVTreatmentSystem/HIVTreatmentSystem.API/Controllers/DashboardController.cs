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
    /// Get monthly appointment statistics for the current year
    /// </summary>
    /// <returns>List of monthly appointment statistics</returns>
    [HttpGet("statistics/appointment-by-month")]
    public async Task<IActionResult> GetAppointmentStatisticsByMonth()
    {
        var result = await _dashboardService.GetAppointmentStatisticsByMonthAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Monthly appointment statistics retrieved successfully",
            Data = result
        });
    }
    
    /// <summary>
    /// Get monthly patient registration statistics for the current year
    /// </summary>
    /// <returns>List of monthly patient statistics</returns>
    [HttpGet("statistics/patient-by-month")] 
    public async Task<IActionResult> GetPatientStatisticsByMonth()
    {
        var result = await _dashboardService.GetPatientStatisticsByMonthAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Monthly patient statistics retrieved successfully",
            Data = result
        });
    }
    
    /// <summary>
    /// Get monthly test result statistics for the current year
    /// </summary>
    /// <returns>List of monthly test result statistics</returns>
    [HttpGet("statistics/test-result-by-month")]
    public async Task<IActionResult> GetTestResultStatisticsByMonth()
    {
        var result = await _dashboardService.GetTestResultStatisticsByMonthAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Monthly test result statistics retrieved successfully",
            Data = result
        });
    }
    [HttpGet("statistics/appointment-by-day")]
    public async Task<IActionResult> GetAppointmentStatisticsByDay()
    {
        var result = await _dashboardService.GetAppointmentStatisticsByDayAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Daily appointment statistics retrieved successfully",
            Data = result
        });
    }
    [HttpGet("statistics/patient-by-day")]
    public async Task<IActionResult> GetPatientStatisticsByDay()
    {
        var result = await _dashboardService.GetPatientStatisticsByDayAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Daily patient statistics retrieved successfully",
            Data = result
        });
    }
    [HttpGet("statistics/test-result-by-day")]
    public async Task<IActionResult> GetTestResultStatisticsByDay()
    {
        var result = await _dashboardService.GetTestResultStatisticsByDayAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Daily test result statistics retrieved successfully",
            Data = result
        });
    }
    [HttpGet("statistics/appointment-by-year")]
    public async Task<IActionResult> GetAppointmentStatisticsByYear()
    {
        var result = await _dashboardService.GetAppointmentStatisticsByYearAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Yearly appointment statistics retrieved successfully",
            Data = result
        });
    }
    [HttpGet("statistics/patient-by-year")]
    public async Task<IActionResult> GetPatientStatisticsByYear()
    {
        var result = await _dashboardService.GetPatientStatisticsByYearAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Yearly patient statistics retrieved successfully",
            Data = result
        });
    }
    [HttpGet("statistics/test-result-by-year")]
    public async Task<IActionResult> GetTestResultStatisticsByYear()
    {
        var result = await _dashboardService.GetTestResultStatisticsByYearAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Yearly test result statistics retrieved successfully",
            Data = result
        });
    }
    [HttpGet("statistics/appointment-by-week")]
    public async Task<IActionResult> GetAppointmentStatisticsByWeek()
    {
        var result = await _dashboardService.GetAppointmentStatisticsByWeekAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Weekly appointment statistics retrieved successfully",
            Data = result
        });
    }
    [HttpGet("statistics/patient-by-week")]
    public async Task<IActionResult> GetPatientStatisticsByWeek()
    {
        var result = await _dashboardService.GetPatientStatisticsByWeekAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Weekly patient statistics retrieved successfully",
            Data = result
        });
    }
    [HttpGet("statistics/test-result-by-week")]
    public async Task<IActionResult> GetTestResultStatisticsByWeek()
    {
        var result = await _dashboardService.GetTestResultStatisticsByWeekAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Weekly test result statistics retrieved successfully",
            Data = result
        });
    }

    [HttpGet("statistics/PatientGenderStatistics")]
    public async Task<IActionResult> GetPatientGenderStatistics()
    {
        var result = await _dashboardService.GetPatientGenderStatisticsAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Patient Gender Statistics",
            Data = result
        });
        
    }
    [HttpGet("statistics/PatientAgeStatistics")]
    public async Task<IActionResult> GetPatientAgeStatistics()
    {
        var result = await _dashboardService.GetPatientAgeStatisticsAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Patient Age Statistics",
            Data = result
        });
    }
    [HttpGet("statistics/PatientPregnancyStatistics")]
    public async Task<IActionResult> GetPatientPregnancyStatistics()
    {
        var result = await _dashboardService.GetPatientPregnancyStatisticsAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Patient Pregnancy Statistics",
            Data = result
        });
    }
    [HttpGet ("statistics/PatientTreatmentStatusStatistics")]
    public async Task<IActionResult> GetPatientTreatmentStatusStatistics()
    {
        var result = await _dashboardService.GetPatientTreatmentStatusStatisticsAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Patient Treatment Status Statistics",
            Data = result
        });
    }
    
}