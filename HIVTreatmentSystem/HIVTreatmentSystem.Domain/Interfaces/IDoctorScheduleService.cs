using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.DTOs;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IMonthlyScheduleService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<DoctorSchedule>> CreateMonthlyScheduleAsync(CreateMonthlyScheduleDTO dto);
    }
} 