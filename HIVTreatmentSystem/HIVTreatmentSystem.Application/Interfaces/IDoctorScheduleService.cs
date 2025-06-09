using HIVTreatmentSystem.Application.Models.DoctorSchedule;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Interfaces
{
    /// <summary>
    /// Service interface for managing doctor schedules.
    /// </summary>
    public interface IDoctorScheduleService
    {
        Task<IEnumerable<DoctorScheduleDto>> GetByDoctorIdAsync(int doctorId);
        Task<IEnumerable<DoctorScheduleDto>> GetByDoctorNameAsync(string doctorName);
        Task<DoctorScheduleDto> GetByIdAsync(int id);
        Task<DoctorScheduleDto> CreateAsync(DoctorScheduleDto dto);
        Task<DoctorScheduleDto> UpdateAsync(int id, DoctorScheduleDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<DoctorScheduleDto>> CreateWeeklyScheduleAsync(CreateWeeklyScheduleDto dto);
    }
} 