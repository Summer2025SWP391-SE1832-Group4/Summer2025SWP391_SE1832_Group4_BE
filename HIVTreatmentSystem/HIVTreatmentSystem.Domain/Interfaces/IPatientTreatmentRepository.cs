using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IPatientTreatmentRepository : IGenericRepository<PatientTreatment, int>
    {
        Task<IEnumerable<PatientTreatment>> GetByPatientIdAsync(int patientId);
        Task<(IEnumerable<PatientTreatment> Items, int TotalCount)> GetPagedAsync(
            string? statusFilter,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        );
    }
}
