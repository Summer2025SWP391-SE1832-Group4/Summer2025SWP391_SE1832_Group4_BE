using HIVTreatmentSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for DoctorSchedule entity.
    /// </summary>
    public interface IDoctorScheduleRepository : IGenericRepository<DoctorSchedule, int>
    {
        /// <summary>
        /// Get all schedules for a specific doctor by ID.
        /// </summary>
        Task<IEnumerable<DoctorSchedule>> GetByDoctorIdAsync(int doctorId);

        /// <summary>
        /// Get all schedules for a specific doctor by name.
        /// </summary>
        Task<IEnumerable<DoctorSchedule>> GetByDoctorNameAsync(string doctorName);
    }
} 