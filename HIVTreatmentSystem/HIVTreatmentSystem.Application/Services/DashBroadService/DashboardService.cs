using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.DashBroad;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services.DashBroadService;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<int> GetTotalPatientsCountAsync()
    {
        return await _dashboardRepository.GetTotalPatientsCountAsync();
    }

    public async Task<(int TotalTests, int PositiveCount, int NegativeCount)> GetTestResultsSummaryAsync()
    {
        return await _dashboardRepository.GetTestResultsSummaryAsync();
    }

    public async Task<List<PatientTreatmentStatusStatisticsResponse>> GetPatientTreatmentStatusStatisticsAsync()
    {
        var data = await _dashboardRepository.GetPatientTreatmentStatusStatisticsAsync();
        return MapToTreatmentStatusStatistics(data);
    }

    private List<PatientTreatmentStatusStatisticsResponse> MapToTreatmentStatusStatistics(List<object> data)
    {
        return data.Select(item =>
        {
            var props = GetObjectProperties(item);
            return new PatientTreatmentStatusStatisticsResponse
            {
                TreatmentStatus = props.TryGetValue("TreatmentStatus", out var status) ? status.ToString() ?? "" : "",
                Count = props.TryGetValue("Count", out var count) ? Convert.ToInt32(count) : 0,
                Percentage = props.TryGetValue("Percentage", out var percentage) ? Convert.ToDouble(percentage) : 0.0
            };
        }).ToList();
    }

    private Dictionary<string, object> GetObjectProperties(object item)
    {
        return item.GetType().GetProperties()
            .ToDictionary(p => p.Name, p => p.GetValue(item) ?? new object());
    }
}