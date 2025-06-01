using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor, int>
    {
        Task<Doctor?> GetDoctorWithDetailsAsync(int doctorId);
        Task<IEnumerable<Doctor>> GetDoctorsBySpecializationAsync(string specialization);
        Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(string department);
        Task<IEnumerable<Doctor>> GetDoctorsWithActivePatientsAsync();
    }
}

