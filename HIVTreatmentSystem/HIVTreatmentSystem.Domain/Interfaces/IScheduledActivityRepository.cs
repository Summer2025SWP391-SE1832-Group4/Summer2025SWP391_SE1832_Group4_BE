
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IScheduledActivityRepository : IGenericRepository<ScheduledActivity, int>
    {
        Task<IEnumerable<ScheduledActivity>> GetByPatientIdAsync(int patientId);
    }
}
