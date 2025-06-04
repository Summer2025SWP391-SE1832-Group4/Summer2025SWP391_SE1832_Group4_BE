using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                DoctorId = dto.DoctorId.HasValue ? dto.DoctorId.Value : throw new System.ArgumentException("DoctorId is required"),
                HospitalName = dto.HospitalName,
                Position = dto.Position,
                FromDate = dto.FromDate.HasValue ? dto.FromDate.Value : throw new System.ArgumentException("FromDate is required"),
                ToDate = dto.ToDate
            };
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return new ExperienceWorkingDto
            {
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
            if (dto.FromDate.HasValue && dto.FromDate.Value != DateTime.MinValue)
                entity.FromDate = dto.FromDate.Value;
            if (dto.ToDate.HasValue && dto.ToDate.Value != DateTime.MinValue)
                entity.ToDate = dto.ToDate;
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return new ExperienceWorkingDto
            {
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
                if (dto.FromDate.HasValue && dto.FromDate.Value != DateTime.MinValue)
                    entity.FromDate = dto.FromDate.Value;
                if (dto.ToDate.HasValue && dto.ToDate.Value != DateTime.MinValue)
                    entity.ToDate = dto.ToDate;
                _repo.Update(entity);
            }
            await _repo.SaveChangesAsync();
            return entities.Select(entity => new ExperienceWorkingDto
            {
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