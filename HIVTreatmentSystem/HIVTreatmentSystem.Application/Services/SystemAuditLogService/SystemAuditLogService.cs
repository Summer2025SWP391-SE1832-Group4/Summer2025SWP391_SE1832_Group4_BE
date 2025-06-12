using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services
{
    /// <summary>
    /// Service implementation for managing system audit logs
    /// </summary>
    public class SystemAuditLogService : ISystemAuditLogService
    {
        private readonly ISystemAuditLogRepository _repo;

        /// <summary>
        /// Constructor for SystemAuditLogService
        /// </summary>
        /// <param name="repo">System audit log repository</param>
        public SystemAuditLogService(ISystemAuditLogRepository repo)
        {
            _repo = repo;
        }

        /// <inheritdoc/>
        public async Task LogAsync(SystemAuditLog log)
        {
            await _repo.AddLogAsync(log);
        }
    }
} 