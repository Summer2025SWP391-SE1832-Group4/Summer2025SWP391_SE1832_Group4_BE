using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Interfaces
{
    /// <summary>
    /// Service interface for managing patient treatments
    /// </summary>
    public interface IPatientTreatmentService
    {
        Task<IEnumerable<PatientTreatmentResponse>> GetAllAsync();
        Task<PatientTreatmentResponse?> GetByIdAsync(int id);
        Task<IEnumerable<PatientTreatmentResponse>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<PatientTreatmentResponse>> GetByDoctorIdAsync(int doctorId);
        Task<PatientTreatmentResponse> CreateAsync(PatientTreatmentRequest request);
        Task<PatientTreatmentResponse> UpdateAsync(int id, PatientTreatmentRequest request);
        Task<bool> DeleteAsync(int id);
    }
} 