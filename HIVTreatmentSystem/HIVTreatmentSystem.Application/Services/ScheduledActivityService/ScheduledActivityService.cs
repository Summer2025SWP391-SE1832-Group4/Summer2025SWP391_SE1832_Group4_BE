
using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services.ScheduledActivityService
{
    public class ScheduledActivityService : IScheduledActivityService
    {
        private readonly IScheduledActivityRepository _repo;
        private readonly IMapper _mapper;

        public ScheduledActivityService(IScheduledActivityRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ScheduledActivityResponse>> GetAllAsync(
            int? patientId,
            string? activityType
        )
        {
            var all = await _repo.GetAllAsync();

            if (patientId.HasValue)
            {
                all = all.Where(a => a.PatientId == patientId);
            }

            if (
                !string.IsNullOrEmpty(activityType)
                && Enum.TryParse<ActivityType>(activityType, true, out var parsedType)
            )
            {
                all = all.Where(a => a.ActivityType == parsedType);
            }

            return _mapper.Map<IEnumerable<ScheduledActivityResponse>>(all);
        }

        public async Task<ScheduledActivityResponse> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException();
            return _mapper.Map<ScheduledActivityResponse>(entity);
        }

        public async Task<ScheduledActivityResponse> CreateAsync(ScheduledActivityRequest request)
        {
            var entity = _mapper.Map<ScheduledActivity>(request);
            await _repo.AddAsync(entity);
            return _mapper.Map<ScheduledActivityResponse>(entity);
        }

        public async Task<ScheduledActivityResponse> UpdateAsync(
            int id,
            ScheduledActivityRequest request
        )
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException();
            _mapper.Map(request, entity);
            _repo.Update(entity);
            return _mapper.Map<ScheduledActivityResponse>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException();
            _repo.Remove(entity);
            return true;
        }
    }
}
