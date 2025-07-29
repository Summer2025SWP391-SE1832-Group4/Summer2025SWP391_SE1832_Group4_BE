using HIVTreatmentSystem.Application.Models.Requests;

namespace HIVTreatmentSystem.Application.Interfaces;

public interface IDashboardService
{
    Task<(IEnumerable<DashboardStatisticsResponse> Items, int TotalCount)> GetAllAsync(
        string? entity,
        string? groupBy,
        DateTime? from,
        DateTime? to
    );
}
