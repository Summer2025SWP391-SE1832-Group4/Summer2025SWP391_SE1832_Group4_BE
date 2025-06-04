using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface ISystemAuditLogService
    {
        Task LogAsync(SystemAuditLog log);
    }
} 