using HIVTreatmentSystem.Domain.Entities;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for managing system audit logs
    /// </summary>
    public interface ISystemAuditLogRepository
    {
        /// <summary>
        /// Add a new audit log entry
        /// </summary>
        /// <param name="log">The audit log entry to add</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task AddLogAsync(SystemAuditLog log);
    }
} 