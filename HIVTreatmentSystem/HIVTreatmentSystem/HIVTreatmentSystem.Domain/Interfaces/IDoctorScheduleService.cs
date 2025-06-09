using HIVTreatmentSystem.Application.Models.DoctorSchedule;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    /// <summary>
    /// Interface for managing doctor schedules
    /// </summary>
    public interface IDoctorScheduleService
    {
        // [DOCTOR SCHEDULE API] Get schedules by doctor ID
        Task<IEnumerable<DoctorScheduleDto>> GetByDoctorIdAsync(int doctorId);

        // [DOCTOR SCHEDULE API] Get schedules by doctor name
        Task<IEnumerable<DoctorScheduleDto>> GetByDoctorNameAsync(string doctorName);

        // [DOCTOR SCHEDULE API] Get schedule by ID
        Task<DoctorScheduleDto> GetByIdAsync(int id);

        // [DOCTOR SCHEDULE API] Create new schedule
        Task<DoctorScheduleDto> CreateAsync(DoctorScheduleDto dto);

        // [DOCTOR SCHEDULE API] Update existing schedule
        Task<DoctorScheduleDto> UpdateAsync(int id, DoctorScheduleDto dto);

        // [DOCTOR SCHEDULE API] Delete schedule
        Task<bool> DeleteAsync(int id);

        // [DOCTOR SCHEDULE API] Create weekly schedules for all doctors
        Task<List<DoctorScheduleDto>> CreateWeeklyScheduleAsync(CreateWeeklyScheduleDto dto);
    }
} 