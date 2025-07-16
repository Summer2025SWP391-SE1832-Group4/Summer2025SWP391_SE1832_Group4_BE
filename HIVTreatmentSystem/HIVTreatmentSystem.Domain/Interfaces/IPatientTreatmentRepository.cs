using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IPatientTreatmentRepository : IGenericRepository<PatientTreatment, int>
    {
        Task<IEnumerable<PatientTreatment>> GetByPatientIdAsync(int patientId);
    }
}
