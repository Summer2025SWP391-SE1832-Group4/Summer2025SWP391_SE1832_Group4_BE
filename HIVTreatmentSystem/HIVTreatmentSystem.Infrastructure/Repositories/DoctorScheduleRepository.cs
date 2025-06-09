using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for DoctorSchedule entity..
    /// </summary>
    public class DoctorScheduleRepository : GenericRepository<DoctorSchedule, int>, IDoctorScheduleRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly HIVDbContext _context;

        public DoctorScheduleRepository(HIVDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoctorSchedule>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.DoctorSchedules
                .Where(s => s.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DoctorSchedule>> GetByDoctorNameAsync(string doctorName)
        {
            return await _context.DoctorSchedules
                .Include(s => s.Doctor)
                .Where(s => s.Doctor.Account.FullName.Contains(doctorName))
                .ToListAsync();
        }
    }
} 