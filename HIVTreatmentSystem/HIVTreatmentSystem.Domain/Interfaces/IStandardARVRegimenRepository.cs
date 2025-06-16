using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IStandardARVRegimenRepository : IGenericRepository<StandardARVRegimen, int>
    {
        Task<(IEnumerable<StandardARVRegimen> Items, int TotalCount)> GetPagedAsync(
            string? regimenNameFilter,
            string? targetPopulationFilter,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        );
    }
}
