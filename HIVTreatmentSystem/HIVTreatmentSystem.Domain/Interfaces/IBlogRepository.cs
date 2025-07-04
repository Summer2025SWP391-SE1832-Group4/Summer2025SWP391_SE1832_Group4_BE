
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IBlogRepository : IGenericRepository<Blog, int>
    {
        Task<(IEnumerable<Blog> Items, int TotalCount)> GetPagedAsync(
            string? titleFilter,
            string? contentFilter,
            int? tagIdFilter,
            DateTime? createdFrom,
            DateTime? createdTo,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        );
    }
}
