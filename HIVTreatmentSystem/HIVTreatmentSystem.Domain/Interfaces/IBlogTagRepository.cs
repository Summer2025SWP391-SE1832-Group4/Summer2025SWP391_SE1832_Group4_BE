
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IBlogTagRepository : IGenericRepository<BlogTag, int>
    {
        Task<(IEnumerable<BlogTag> Items, int TotalCount)> GetPagedAsync(
            string? nameFilter,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        );
    }
}
