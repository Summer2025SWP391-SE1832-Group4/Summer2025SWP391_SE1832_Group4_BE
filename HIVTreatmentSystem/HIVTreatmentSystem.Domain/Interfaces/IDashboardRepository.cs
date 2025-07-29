namespace HIVTreatmentSystem.Domain.Interfaces;

public interface IDashboardRepository
{
    // Get total patient count
    Task<int> GetTotalPatientsCountAsync();
    
    // Get test result statistics
    Task<(int TotalTests, int PositiveCount, int NegativeCount)> GetTestResultsSummaryAsync();
    
    // Get treatment status statistics
    Task<List<object>> GetPatientTreatmentStatusStatisticsAsync();
}
