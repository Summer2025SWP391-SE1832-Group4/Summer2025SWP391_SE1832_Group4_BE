using HIVTreatmentSystem.Domain.Entities;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface ISystemAuditLogRepository : IGenericRepository<SystemAuditLog, long>
    {
        Task AddLogAsync(SystemAuditLog log);
    }
} 