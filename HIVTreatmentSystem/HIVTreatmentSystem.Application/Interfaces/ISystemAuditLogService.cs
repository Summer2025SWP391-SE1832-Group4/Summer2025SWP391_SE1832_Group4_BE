using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Application.Interfaces
{
    /// <summary>
    /// Service interface for managing system audit logs
    /// </summary>
    public interface ISystemAuditLogService
    {
        /// <summary>
        /// Log a system audit event
        /// </summary>
        /// <param name="log">The audit log entry to add</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task LogAsync(SystemAuditLog log);
    }
} 