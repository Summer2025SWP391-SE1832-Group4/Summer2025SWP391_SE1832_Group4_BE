using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IPatientTreatmentService
    {
        Task<IEnumerable<PatientTreatmentResponse>> GetAllAsync();
        Task<PatientTreatmentResponse> GetByIdAsync(int id);
        Task<IEnumerable<PatientTreatmentResponse>> GetByPatientIdAsync(int patientId);
        Task<PatientTreatmentResponse> CreateAsync(PatientTreatmentRequest request);
        Task<PatientTreatmentResponse> UpdateAsync(int id, PatientTreatmentRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
