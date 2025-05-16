using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient> GetPatientWithDetailsAsync(Guid id);
        Task<IEnumerable<Patient>> GetPatientsByDoctorAsync(Guid doctorId);
        Task<IEnumerable<Patient>> GetPatientsByTreatmentTypeAsync(string treatmentType);
        Task<IEnumerable<Patient>> GetPatientsByHIVStatusAsync(string hivStatus);
    }
} 