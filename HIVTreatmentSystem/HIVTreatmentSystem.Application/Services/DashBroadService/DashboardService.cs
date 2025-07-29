using System.Dynamic;
using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
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
}
