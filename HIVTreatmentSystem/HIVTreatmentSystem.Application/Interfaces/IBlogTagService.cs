
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IBlogTagService
    {
        Task<(IEnumerable<BlogTagResponse> Items, int TotalCount)> GetAllAsync(
            string? nameFilter,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default
        );

        Task<BlogTagResponse> GetByIdAsync(int id, CancellationToken ct = default);

        Task<BlogTagResponse> CreateAsync(BlogTagRequest request, CancellationToken ct = default);

        Task<BlogTagResponse> UpdateAsync(
            int id,
            BlogTagRequest request,
            CancellationToken ct = default
        );

        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
