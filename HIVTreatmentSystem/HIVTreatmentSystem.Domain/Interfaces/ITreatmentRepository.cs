using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface ITreatmentRepository : IGenericRepository<Treatment, Guid>
    {
        Task<Treatment> GetTreatmentWithDetailsAsync(Guid id);
        Task<IEnumerable<Treatment>> GetTreatmentsByPatientAsync(Guid patientId);
        Task<IEnumerable<Treatment>> GetTreatmentsByDoctorAsync(Guid doctorId);
        Task<IEnumerable<Treatment>> GetActiveTreatmentsAsync();
        Task<IEnumerable<Treatment>> GetTreatmentsByTypeAsync(string treatmentType);
    }
}

