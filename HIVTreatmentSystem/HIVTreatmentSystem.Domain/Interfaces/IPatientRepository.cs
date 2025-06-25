using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
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

        Task<Patient?> GetByIdAsync(int id);
        Task UpdateAsync(Patient patient);

        Task<IEnumerable<Patient>> GetAllPatientsAsync(
            int? accountId,
            DateTime? dateOfBirth,
            Gender? gender,
            string? address,
            DateTime? hivDiagnosisDate,
            string? consentInformation,
            bool isDescending = false,
            string? sortBy = "");

        Task DeleteAsync(Patient patient); 
    }
}

