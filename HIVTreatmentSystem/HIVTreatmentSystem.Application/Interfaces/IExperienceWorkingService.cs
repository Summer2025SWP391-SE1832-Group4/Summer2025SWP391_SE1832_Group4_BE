using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Models.ExperienceWorking;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IExperienceWorkingService
    {
        Task<IEnumerable<ExperienceWorkingDto>> GetByDoctorIdAsync(int doctorId);
        Task<ExperienceWorkingDto> GetByIdAsync(int id);
        Task<ExperienceWorkingDto> CreateAsync(ExperienceWorkingDto dto);
        Task<ExperienceWorkingDto> UpdateAsync(int id, ExperienceWorkingDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ExperienceWorkingDto>> UpdateByDoctorIdAsync(int doctorId, ExperienceWorkingDoctorDTO dto);
    }
} 