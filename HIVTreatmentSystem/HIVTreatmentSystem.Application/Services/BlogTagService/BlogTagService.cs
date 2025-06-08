using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services.BlogTagService
{
    public class BlogTagService : IBlogTagService
    {
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IMapper _mapper;

        public BlogTagService(IBlogTagRepository blogTagRepository, IMapper mapper)
        {
            _blogTagRepository = blogTagRepository;
            _mapper = mapper;
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
            var (entities, total) = await _blogTagRepository.GetPagedAsync(
                nameFilter,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            var items = _mapper.Map<IEnumerable<BlogTagResponse>>(entities);
            return (items, total);
        }

        public async Task<BlogTagResponse> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _blogTagRepository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("không tìm thấy thẻ");
            return _mapper.Map<BlogTagResponse>(entity);
        }

        public async Task<BlogTagResponse> CreateAsync(
            BlogTagRequest request,
            CancellationToken ct = default
        )
        {
            var entity = _mapper.Map<BlogTag>(request);
            await _blogTagRepository.AddAsync(entity);
            return _mapper.Map<BlogTagResponse>(entity);
        }

        public async Task<BlogTagResponse> UpdateAsync(
            int id,
            BlogTagRequest request,
            CancellationToken ct = default
        )
        {
            var entity = await _blogTagRepository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("không tìm thấy thẻ");
            _mapper.Map(request, entity);
            _blogTagRepository.Update(entity);
            return _mapper.Map<BlogTagResponse>(entity);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _blogTagRepository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("không tìm thấy thẻ");
            _blogTagRepository.Remove(entity);
        }
    }
}
