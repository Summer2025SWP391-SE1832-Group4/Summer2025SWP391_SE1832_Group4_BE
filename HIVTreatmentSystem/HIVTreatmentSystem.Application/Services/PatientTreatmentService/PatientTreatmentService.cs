using System.Linq;
using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Repositories;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services.PatientTreatmentService
{
    public class PatientTreatmentService : IPatientTreatmentService
    {
        private readonly IPatientTreatmentRepository _repo;
        private readonly IMapper _mapper;

        public PatientTreatmentService(IPatientTreatmentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientTreatmentResponse>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientTreatmentResponse>>(list);
        }

        public async Task<PatientTreatmentResponse> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException();
            }
            return _mapper.Map<PatientTreatmentResponse>(entity);
        }

        public async Task<IEnumerable<PatientTreatmentResponse>> GetByPatientIdAsync(int patientId)
        {
            var result = await _repo.GetByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<PatientTreatmentResponse>>(result);
        }

        public async Task<PatientTreatmentResponse> CreateAsync(PatientTreatmentRequest request)
        {
            var entity = _mapper.Map<PatientTreatment>(request);
            await _repo.AddAsync(entity);
            return _mapper.Map<PatientTreatmentResponse>(entity);
        }

        public async Task<PatientTreatmentResponse> UpdateAsync(
            int id,
            PatientTreatmentRequest request
        )
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException();
            }
            _mapper.Map(request, entity);
            _repo.Update(entity);
            return _mapper.Map<PatientTreatmentResponse>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException();
            }
            _repo.Remove(entity);
            return true;
        }
    }
}
