using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Interfaces;

public interface IDashboardService
{
    Task<(IEnumerable<DashboardStatisticsResponse> Items, int TotalCount)> GetAllAsync(
        string? entity,
        string? groupBy,
        DateTime? from,
        DateTime? to
    );
    Task<TestResultSummaryResponse> GetTestResultSummaryAsync();

    Task<(IEnumerable<PatientTreatmentResponse> Items, int TotalCount)> GetPatientTreatmentsAsync(
        string? statusFilter,
        string? sortBy,
        bool sortDesc,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );
    Task<TreatmentStatusCountResponse> GetTreatmentStatusCountAsync();
}
