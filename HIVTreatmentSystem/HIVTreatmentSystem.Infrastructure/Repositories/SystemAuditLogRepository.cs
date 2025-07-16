using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class SystemAuditLogRepository : GenericRepository<SystemAuditLog, long>, ISystemAuditLogRepository
    {
        private readonly HIVDbContext _context;
        public SystemAuditLogRepository(HIVDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddLogAsync(SystemAuditLog log)
        {
            await _context.SystemAuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
} 