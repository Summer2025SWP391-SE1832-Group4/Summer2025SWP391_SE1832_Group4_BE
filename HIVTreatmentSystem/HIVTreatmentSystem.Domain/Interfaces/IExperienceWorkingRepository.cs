using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IExperienceWorkingRepository : IGenericRepository<ExperienceWorking, int>
    {
        Task<IEnumerable<ExperienceWorking>> GetByDoctorIdAsync(int doctorId);
        Task SaveChangesAsync();
    }
} 