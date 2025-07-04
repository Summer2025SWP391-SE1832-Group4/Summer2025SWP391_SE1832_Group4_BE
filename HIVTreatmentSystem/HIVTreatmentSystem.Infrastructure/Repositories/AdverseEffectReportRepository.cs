using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class AdverseEffectReportRepository : IAdverseEffectReportRepository
    {

        private readonly HIVDbContext _context;

        public AdverseEffectReportRepository(HIVDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AdverseEffectReport>> GetAllAsync(
            int? accountId,
            DateOnly? dateOccurred,
            AdverseEffectSeverityEnum? severity,
            AdverseEffectReportStatusEnum? status,
            DateOnly? startDate,
            DateOnly? endDate,
            bool isDescending,
            string? sortBy
        )
        {
            var query = _context
                .AdverseEffectReports
                .Include(a => a.Patient)
                .ThenInclude(p => p.Account)
                .AsQueryable();

            if (accountId.HasValue)
            {
                query = query.Where(r => r.Patient.AccountId == accountId);
            }

            if (dateOccurred.HasValue)
            {
                query = query.Where(r => r.DateOccurred == dateOccurred);
            }

            if (severity.HasValue)
            {
                query = query.Where(r => r.Severity == severity);
            }

            if (startDate.HasValue)
            {
                query = query.Where(r => r.DateOccurred >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(r => r.DateOccurred <= endDate);
            }

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status);
            }

            query = sortBy?.ToLower() switch
            {
                "accountId" => isDescending ?
                    query.OrderByDescending(r => r.Patient.AccountId) :
                    query.OrderBy(r => r.Patient.AccountId),

                "dateOccurred" => isDescending ?
                    query.OrderByDescending(r => r.DateOccurred) :
                    query.OrderBy(r => r.DateOccurred),

                "severity" => isDescending ?
                    query.OrderByDescending(r => r.Severity) :
                    query.OrderBy(r => r.Severity),

                _ => isDescending ?
                query.OrderByDescending(r => r.DateOccurred) :
                query.OrderBy(r => r.DateOccurred)
            };

            return await query.ToListAsync();

        }

        public async Task<AdverseEffectReport?> GetByIdAsync(int id)
        {
            return await _context
                .AdverseEffectReports
                .Include(a => a.Patient)
                .ThenInclude(p => p.Account)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> AddAsync(AdverseEffectReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }
            _context.AdverseEffectReports.Add(report);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task DeleteAsync(AdverseEffectReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }
            _context.AdverseEffectReports.Remove(report);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(AdverseEffectReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }
            _context.AdverseEffectReports.Update(report);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
