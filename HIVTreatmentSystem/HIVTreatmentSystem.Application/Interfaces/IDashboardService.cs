using HIVTreatmentSystem.Application.Models.DashBroad;

namespace HIVTreatmentSystem.Application.Interfaces;

public interface IDashboardService
{
    // Get total patient count
    Task<int> GetTotalPatientsCountAsync();
    
    // Get test result statistics
    Task<(int TotalTests, int PositiveCount, int NegativeCount)> GetTestResultsSummaryAsync();
    
    // Get treatment status statistics
    Task<List<PatientTreatmentStatusStatisticsResponse>> GetPatientTreatmentStatusStatisticsAsync();
}