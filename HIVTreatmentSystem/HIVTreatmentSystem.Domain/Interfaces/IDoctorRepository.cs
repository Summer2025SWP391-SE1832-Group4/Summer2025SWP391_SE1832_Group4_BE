using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

    }
}

