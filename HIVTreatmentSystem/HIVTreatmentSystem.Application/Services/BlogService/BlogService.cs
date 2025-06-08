using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services.BlogService
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public BlogService(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<BlogResponse> Items, int TotalCount)> GetAllAsync(
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
        )
        {
            var (entities, total) = await _blogRepository.GetPagedAsync(
                titleFilter,
                contentFilter,
                tagIdFilter,
                createdFrom,
                createdTo,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );

            var items = _mapper.Map<IEnumerable<BlogResponse>>(entities);
            return (items, total);
        }

        public async Task<BlogResponse> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _blogRepository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("không tìm thấy blog");

            return _mapper.Map<BlogResponse>(entity);
        }

        public async Task<BlogResponse> CreateAsync(BlogRequest req, CancellationToken ct = default)
        {
            var entity = _mapper.Map<Blog>(req);
            entity.CreatedAt = DateTime.UtcNow;

            await _blogRepository.AddAsync(entity);

            await Task.Run(() => _ = entity.BlogTag, ct);

            return _mapper.Map<BlogResponse>(entity);
        }

        public async Task<BlogResponse> UpdateAsync(
            int id,
            BlogRequest req,
            CancellationToken ct = default
        )
        {
            var entity = await _blogRepository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("không tìm thấy blog");

            _mapper.Map(req, entity);
            _blogRepository.Update(entity);
            await Task.Run(() => _ = entity.BlogTag, ct);

            return _mapper.Map<BlogResponse>(entity);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _blogRepository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("không tìm thấy blog");

            _blogRepository.Remove(entity);
        }
    }
}
