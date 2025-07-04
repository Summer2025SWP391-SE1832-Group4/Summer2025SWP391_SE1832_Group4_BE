using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IScheduledActivityRepository : IGenericRepository<ScheduledActivity, int>
    {
        Task<IEnumerable<ScheduledActivity>> GetByPatientIdAsync(int patientId);
    }
}
