using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services
{
    public class SystemAuditLogService : ISystemAuditLogService
    {
        private readonly ISystemAuditLogRepository _repo;
        public SystemAuditLogService(ISystemAuditLogRepository repo)
        {
            _repo = repo;
        }
        public async Task LogAsync(SystemAuditLog log)
        {
            await _repo.AddLogAsync(log);
        }
    }
} 