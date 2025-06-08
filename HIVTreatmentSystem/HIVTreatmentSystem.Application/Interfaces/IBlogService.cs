using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IBlogService
    {
        Task<(IEnumerable<BlogResponse> Items, int TotalCount)> GetAllAsync(
            string? titleFilter,
            string? contentFilter,
            int? tagIdFilter,
            DateTime? createdFrom,
            DateTime? createdTo,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default
        );

        Task<BlogResponse> GetByIdAsync(int id, CancellationToken ct = default);

        Task<BlogResponse> CreateAsync(BlogRequest req, CancellationToken ct = default);

        Task<BlogResponse> UpdateAsync(int id, BlogRequest req, CancellationToken ct = default);

        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
