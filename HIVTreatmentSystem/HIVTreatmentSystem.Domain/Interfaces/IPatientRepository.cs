using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient> GetPatientWithDetailsAsync(int id);
        Task<IEnumerable<Patient>> GetPatientsByDoctorAsync(int doctorId);
        Task<IEnumerable<Patient>> GetPatientsByTreatmentTypeAsync(string treatmentType);
        Task<IEnumerable<Patient>> GetPatientsByHIVStatusAsync(string hivStatus);
    }
}

