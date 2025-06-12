using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Standard ARV Regimen operations
    /// </summary>
    public class StandardARVRegimenRepository : IStandardARVRegimenRepository
    {
        private readonly HIVDbContext _context;

        public StandardARVRegimenRepository(HIVDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<StandardARVRegimen>> GetAllAsync()
        {
            return await _context.StandardARVRegimens
                .Include(r => r.PatientTreatments)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<StandardARVRegimen?> GetByIdAsync(int id)
        {
            return await _context.StandardARVRegimens
                .Include(r => r.PatientTreatments)
                .FirstOrDefaultAsync(r => r.RegimenId == id);
        }

        /// <inheritdoc/>
        public async Task<StandardARVRegimen> CreateAsync(StandardARVRegimen regimen)
        {
            _context.StandardARVRegimens.Add(regimen);
            await _context.SaveChangesAsync();
            return regimen;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(StandardARVRegimen regimen)
        {
            _context.StandardARVRegimens.Update(regimen);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(StandardARVRegimen regimen)
        {
            _context.StandardARVRegimens.Remove(regimen);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 