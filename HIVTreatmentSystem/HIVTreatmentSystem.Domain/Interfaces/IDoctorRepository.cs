using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using System.Linq.Expressions;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor, int>
    {
        Task<IEnumerable<Doctor>> FindAsync(Expression<Func<Doctor, bool>> predicate);
        Task<Doctor?> GetByIdAsync(int id);
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor?> GetDoctorWithDetailsAsync(int doctorId);
        Task<IEnumerable<Doctor>> GetDoctorsBySpecialtyAsync(DoctorSpecialtyEnum specialty);
        Task<IEnumerable<Doctor>> GetDoctorsWithSchedulesAsync();
        Task<List<Doctor>> GetDoctorsNotInIdsAsync(List<int> excludedDoctorIds);
        Task<Doctor?> GetByAccountIdAsync(int accountId);
        Task<List<Doctor>> GetAvailableDoctorsAsync(List<int> excludedDoctorIds, DoctorSpecialtyEnum specialty);

        Task DeleteAsync(Doctor doctor);

        Task UpdateAsync(Doctor doctor);

    }
}

