
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.ExperienceWorking;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services
{
    public class ExperienceWorkingService : IExperienceWorkingService
    {
        private readonly IExperienceWorkingRepository _repo;

        public ExperienceWorkingService(IExperienceWorkingRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ExperienceWorkingDto>> GetByDoctorIdAsync(int doctorId)
        {
            var list = await _repo.GetByDoctorIdAsync(doctorId);
            return list.Select(x => new ExperienceWorkingDto
            {
                Id = x.Id,
                DoctorId = x.DoctorId,
                HospitalName = x.HospitalName,
                Position = x.Position,
                FromDate = x.FromDate,
                ToDate = x.ToDate
            });
        }

        public async Task<ExperienceWorkingDto> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return null;
            return new ExperienceWorkingDto
            {
                Id = x.Id,
                DoctorId = x.DoctorId,
                HospitalName = x.HospitalName,
                Position = x.Position,
                FromDate = x.FromDate,
                ToDate = x.ToDate
            };
        }

        public async Task<ExperienceWorkingDto> CreateAsync(ExperienceWorkingDto dto)
        {
            var entity = new ExperienceWorking
            {
                DoctorId = dto.DoctorId,
                HospitalName = dto.HospitalName,
                Position = dto.Position,
                FromDate = dto.FromDate ?? DateTime.Now,
                ToDate = dto.ToDate
            };
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return new ExperienceWorkingDto
            {
                Id = entity.Id,
                DoctorId = entity.DoctorId,
                HospitalName = entity.HospitalName,
                Position = entity.Position,
                FromDate = entity.FromDate,
                ToDate = entity.ToDate
            };
        }

        public async Task<ExperienceWorkingDto> UpdateAsync(int id, ExperienceWorkingDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;
            if (!string.IsNullOrEmpty(dto.HospitalName) && dto.HospitalName != "string")
                entity.HospitalName = dto.HospitalName;
            if (!string.IsNullOrEmpty(dto.Position) && dto.Position != "string")
                entity.Position = dto.Position;
            if (dto.FromDate.HasValue)
                entity.FromDate = dto.FromDate.Value;
            if (dto.ToDate.HasValue)
                entity.ToDate = dto.ToDate;
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return new ExperienceWorkingDto
            {
                Id = entity.Id,
                DoctorId = entity.DoctorId,
                HospitalName = entity.HospitalName,
                Position = entity.Position,
                FromDate = entity.FromDate,
                ToDate = entity.ToDate
            };
        }

        public async Task<IEnumerable<ExperienceWorkingDto>> UpdateByDoctorIdAsync(int doctorId,
            ExperienceWorkingDoctorDTO dto)
        {
            var entities = (await _repo.GetByDoctorIdAsync(doctorId)).ToList();
            foreach (var entity in entities)
            {
                if (!string.IsNullOrEmpty(dto.HospitalName) && dto.HospitalName != "string")
                    entity.HospitalName = dto.HospitalName;
                if (!string.IsNullOrEmpty(dto.Position) && dto.Position != "string")
                    entity.Position = dto.Position;
                if (dto.FromDate.HasValue)
                    entity.FromDate = dto.FromDate.Value;
                if (dto.ToDate.HasValue)
                    entity.ToDate = dto.ToDate;
                _repo.Update(entity);
            }
            await _repo.SaveChangesAsync();
            return entities.Select(entity => new ExperienceWorkingDto
            {
                Id = entity.Id,
                DoctorId = entity.DoctorId,
                HospitalName = entity.HospitalName,
                Position = entity.Position,
                FromDate = entity.FromDate,
                ToDate = entity.ToDate
            });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            _repo.Remove(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
} 