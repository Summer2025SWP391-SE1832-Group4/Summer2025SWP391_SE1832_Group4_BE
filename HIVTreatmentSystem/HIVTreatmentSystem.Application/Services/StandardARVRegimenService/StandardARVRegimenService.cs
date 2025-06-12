using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services
{
    /// <summary>
    /// Service implementation for Standard ARV Regimen operations
    /// </summary>
    public class StandardARVRegimenService : IStandardARVRegimenService
    {
        private readonly IStandardARVRegimenRepository _regimenRepository;
        private readonly IMapper _mapper;

        public StandardARVRegimenService(IStandardARVRegimenRepository regimenRepository, IMapper mapper)
        {
            _regimenRepository = regimenRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<StandardARVRegimenResponse>> GetAllAsync()
        {
            var regimens = await _regimenRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StandardARVRegimenResponse>>(regimens);
        }

        /// <inheritdoc/>
        public async Task<StandardARVRegimenResponse?> GetByIdAsync(int id)
        {
            var regimen = await _regimenRepository.GetByIdAsync(id);
            return regimen == null ? null : _mapper.Map<StandardARVRegimenResponse>(regimen);
        }

        /// <inheritdoc/>
        public async Task<StandardARVRegimenResponse> CreateAsync(StandardARVRegimenRequest request)
        {
            var regimen = _mapper.Map<StandardARVRegimen>(request);
            var createdRegimen = await _regimenRepository.CreateAsync(regimen);
            return _mapper.Map<StandardARVRegimenResponse>(createdRegimen);
        }

        /// <inheritdoc/>
        public async Task<StandardARVRegimenResponse> UpdateAsync(int id, StandardARVRegimenRequest request)
        {
            var existingRegimen = await _regimenRepository.GetByIdAsync(id);
            if (existingRegimen == null)
                throw new ArgumentException($"Regimen with ID {id} not found.");

            _mapper.Map(request, existingRegimen);
            await _regimenRepository.UpdateAsync(existingRegimen);
            return _mapper.Map<StandardARVRegimenResponse>(existingRegimen);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            var regimen = await _regimenRepository.GetByIdAsync(id);
            if (regimen == null)
                throw new ArgumentException($"Regimen with ID {id} not found.");

            return await _regimenRepository.DeleteAsync(regimen);
        }
    }
} 