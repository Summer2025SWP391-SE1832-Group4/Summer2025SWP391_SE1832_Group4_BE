using System.Dynamic;
using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services.DashBroadService;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repo;
    private readonly IMapper _mapper;

    public DashboardService(IDashboardRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<(IEnumerable<DashboardStatisticsResponse> Items, int TotalCount)> GetAllAsync(
        string? entity,
        string? groupBy,
        DateTime? from,
        DateTime? to
    )
    {
        var (entities, total) = await _repo.GetAllAsync(entity, groupBy, from, to);
        var items = _mapper.Map<IEnumerable<DashboardStatisticsResponse>>(entities);
        return (items, total);
    }

    public async Task<TestResultSummaryResponse> GetTestResultSummaryAsync()
    {
        var raw = await _repo.GetTestResultSummaryAsync();
        // ánh x? t? anonymous object sang DTO
        var props = new ExpandoObject() as IDictionary<string, object?>;
        foreach (var p in raw.GetType().GetProperties())
        {
            props[p.Name] = p.GetValue(raw);
        }

        return new TestResultSummaryResponse
        {
            TotalTests = Convert.ToInt32(props["TotalTests"]),
            PositiveCount = Convert.ToInt32(props["PositiveCount"]),
            NegativeCount = Convert.ToInt32(props["NegativeCount"]),
            PositivePercentage = Convert.ToDouble(props["PositivePercentage"]),
            NegativePercentage = Convert.ToDouble(props["NegativePercentage"]),
        };
    }
}
