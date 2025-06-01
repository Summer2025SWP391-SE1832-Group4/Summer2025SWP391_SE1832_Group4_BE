using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface ITreatmentRepository : IGenericRepository<PatientTreatment, int>
    {
        Task<PatientTreatment?> GetTreatmentWithDetailsAsync(int patientTreatmentId);
        Task<IEnumerable<PatientTreatment>> GetTreatmentsByPatientAsync(int patientId);
        Task<IEnumerable<PatientTreatment>> GetTreatmentsByDoctorAsync(int doctorId);
        Task<IEnumerable<PatientTreatment>> GetActiveTreatmentsAsync();
        Task<IEnumerable<PatientTreatment>> GetTreatmentsByStatusAsync(string status);
    }
}

