using HIVTreatmentSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient, int>
    {
        Task<Patient?> GetPatientWithDetailsAsync(int patientId);
        Task<IEnumerable<Patient>> GetPatientsByDoctorAsync(int doctorId);
        Task<IEnumerable<Patient>> GetPatientsByTreatmentTypeAsync(string treatmentType);
        Task<IEnumerable<Patient>> GetPatientsByHIVStatusAsync(string hivStatus);
        Task<Patient?> GetByAccountIdAsync(int accountId);

        Task<bool> AnyAsync(string patientCode);


    }
}

