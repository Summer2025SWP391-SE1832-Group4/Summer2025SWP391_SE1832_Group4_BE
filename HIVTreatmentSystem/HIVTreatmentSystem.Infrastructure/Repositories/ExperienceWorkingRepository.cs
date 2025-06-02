using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class ExperienceWorkingRepository : GenericRepository<ExperienceWorking, int>, IExperienceWorkingRepository
    {
        private readonly HIVDbContext _context;
        public ExperienceWorkingRepository(HIVDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExperienceWorking>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.ExperienceWorkings.Where(x => x.DoctorId == doctorId).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 