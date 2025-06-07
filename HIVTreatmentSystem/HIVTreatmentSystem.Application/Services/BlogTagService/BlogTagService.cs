using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services.BlogTagService
{
    public class BlogTagService : IBlogTagService
    {
        private readonly IBlogTagRepository _repo;

        public BlogTagService(IBlogTagRepository repo)
        {
            _repo = repo;
        }

        public async Task<(IEnumerable<BlogTagResponse> Items, int TotalCount)> GetAllAsync(
            string? nameFilter,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default
        )
        {
            var (entities, total) = await _repo.GetPagedAsync(
                nameFilter,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            var items = entities.Select(t => new BlogTagResponse
            {
                BlogTagId = t.BlogTagId,
                Name = t.Name,
                Description = t.Description,
            });
            return (items, total);
        }

        public async Task<BlogTagResponse> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null)
                throw new KeyNotFoundException("không tìm thấy thẻ");
            return new BlogTagResponse
            {
                BlogTagId = t.BlogTagId,
                Name = t.Name,
                Description = t.Description,
            };
        }

        public async Task<BlogTagResponse> CreateAsync(
            BlogTagRequest request,
            CancellationToken ct = default
        )
        {
            var entity = new BlogTag { Name = request.Name, Description = request.Description };
            await _repo.AddAsync(entity);
            return new BlogTagResponse
            {
                BlogTagId = entity.BlogTagId,
                Name = entity.Name,
                Description = entity.Description,
            };
        }

        public async Task<BlogTagResponse> UpdateAsync(
            int id,
            BlogTagRequest request,
            CancellationToken ct = default
        )
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null)
                throw new KeyNotFoundException("không tìm thấy thẻ");
            t.Name = request.Name;
            t.Description = request.Description;
            _repo.Update(t);
            return new BlogTagResponse
            {
                BlogTagId = t.BlogTagId,
                Name = t.Name,
                Description = t.Description,
            };
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null)
                throw new KeyNotFoundException("không tìm thấy thẻ");
            _repo.Remove(t);
        }
    }
}
