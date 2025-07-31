using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces;

public interface IDashboardRepository
{
    Task<(IEnumerable<DashboardStatistics> Items, int TotalCount)> GetAllAsync(
        string? entity,
        string? groupBy,
        DateTime? from,
        DateTime? to
    );
    Task<object> GetTestResultSummaryAsync();
    Task<IEnumerable<(string Status, int Count)>> GetTreatmentStatusCountsAsync();
}
