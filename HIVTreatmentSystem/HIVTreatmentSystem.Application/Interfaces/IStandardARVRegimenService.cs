using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IStandardARVRegimenService
    {
        Task<(IEnumerable<StandardARVRegimenResponse> Items, int TotalCount)> GetAllAsync(
            string? regimenNameFilter,
            string? targetPopulationFilter,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default
        );

        Task<StandardARVRegimenResponse> GetByIdAsync(int id, CancellationToken ct = default);

        Task<StandardARVRegimenResponse> CreateAsync(
            StandardARVRegimenRequest req,
            CancellationToken ct = default
        );

        Task<StandardARVRegimenResponse> UpdateAsync(
            int id,
            StandardARVRegimenRequest req,
            CancellationToken ct = default
        );

        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
