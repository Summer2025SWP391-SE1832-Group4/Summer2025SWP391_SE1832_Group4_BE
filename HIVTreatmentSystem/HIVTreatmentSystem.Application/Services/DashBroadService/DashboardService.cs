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
    private readonly IPatientTreatmentRepository _treatmentRepo;

    public DashboardService(
        IDashboardRepository repo,
        IMapper mapper,
        IPatientTreatmentRepository treatmentRepo
    )
    {
        _repo = repo;
        _mapper = mapper;
        _treatmentRepo = treatmentRepo;
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

    public async Task<(
        IEnumerable<PatientTreatmentResponse> Items,
        int TotalCount
    )> GetPatientTreatmentsAsync(
        string? statusFilter,
        string? sortBy,
        bool sortDesc,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var (entities, total) = await _treatmentRepo.GetPagedAsync(
            statusFilter,
            sortBy,
            sortDesc,
            pageNumber,
            pageSize
        );
        var items = _mapper.Map<IEnumerable<PatientTreatmentResponse>>(entities);
        return (items, total);
    }
}
