using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class ScheduledActivityRepository
        : GenericRepository<ScheduledActivity, int>,
            IScheduledActivityRepository
    {
        private readonly HIVDbContext _context;

        public ScheduledActivityRepository(HIVDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ScheduledActivity>> GetByPatientIdAsync(int patientId)
        {
            return await _context
                .ScheduledActivities.Include(a => a.Patient)
                .Include(a => a.CreatedByStaff)
                .ThenInclude(s => s.Account)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }
    }
}
