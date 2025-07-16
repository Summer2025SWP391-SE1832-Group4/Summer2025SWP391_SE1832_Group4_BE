using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services
{
    public class StandardARVRegimenService : IStandardARVRegimenService
    {
        private readonly IStandardARVRegimenRepository _standardARVRegimenRepository;
        private readonly IMapper _mapper;

        public StandardARVRegimenService(
            IStandardARVRegimenRepository standardARVRegimenRepository,
            IMapper mapper
        )
        {
            _standardARVRegimenRepository = standardARVRegimenRepository;
            _mapper = mapper;
        }

        public async Task<(
            IEnumerable<StandardARVRegimenResponse> Items,
            int TotalCount
        )> GetAllAsync(
            string? regimenNameFilter,
            string? targetPopulationFilter,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default
        )
        {
            var (ents, total) = await _standardARVRegimenRepository.GetPagedAsync(
                regimenNameFilter,
                targetPopulationFilter,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            var items = _mapper.Map<IEnumerable<StandardARVRegimenResponse>>(ents);
            return (items, total);
        }

        public async Task<StandardARVRegimenResponse> GetByIdAsync(
            int id,
            CancellationToken ct = default
        )
        {
            var e = await _standardARVRegimenRepository.GetByIdAsync(id);
            if (e == null)
                throw new KeyNotFoundException("không tìm thấy regimen");
            return _mapper.Map<StandardARVRegimenResponse>(e);
        }

        public async Task<StandardARVRegimenResponse> CreateAsync(
            StandardARVRegimenRequest req,
            CancellationToken ct = default
        )
        {
            var e = _mapper.Map<Domain.Entities.StandardARVRegimen>(req);
            await _standardARVRegimenRepository.AddAsync(e);
            return _mapper.Map<StandardARVRegimenResponse>(e);
        }

        public async Task<StandardARVRegimenResponse> UpdateAsync(
            int id,
            StandardARVRegimenRequest req,
            CancellationToken ct = default
        )
        {
            var e = await _standardARVRegimenRepository.GetByIdAsync(id);
            if (e == null)
                throw new KeyNotFoundException("không tìm thấy regimen");
            _mapper.Map(req, e);
            _standardARVRegimenRepository.Update(e);
            return _mapper.Map<StandardARVRegimenResponse>(e);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var e = await _standardARVRegimenRepository.GetByIdAsync(id);
            if (e == null)
                throw new KeyNotFoundException("không tìm thấy regimen");
            _standardARVRegimenRepository.Remove(e);
        }
    }
}
