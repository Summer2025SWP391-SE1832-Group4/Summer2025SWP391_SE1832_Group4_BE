using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Interfaces
{
    /// <summary>
    /// Service interface for Medical Record operations
    /// </summary>
    public interface IMedicalRecordService
    {
        /// <summary>
        /// Get all medical records
        /// </summary>
        Task<IEnumerable<MedicalRecordResponse>> GetAllAsync();

        /// <summary>
        /// Get a medical record by its ID
        /// </summary>
        /// <param name="id">The ID of the medical record to retrieve</param>
        Task<MedicalRecordResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Get medical records by patient ID
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        Task<IEnumerable<MedicalRecordResponse>> GetByPatientIdAsync(int patientId);

        /// <summary>
        /// Get medical records by doctor ID
        /// </summary>
        /// <param name="doctorId">The ID of the doctor</param>
        Task<IEnumerable<MedicalRecordResponse>> GetByDoctorIdAsync(int doctorId);

        /// <summary>
        /// Create a new medical record
        /// </summary>
        /// <param name="request">The medical record data to create</param>
        Task<MedicalRecordResponse> CreateAsync(MedicalRecordRequest request);

        /// <summary>
        /// Update an existing medical record
        /// </summary>
        /// <param name="id">The ID of the medical record to update</param>
        /// <param name="request">The updated medical record data</param>
        Task<MedicalRecordResponse> UpdateAsync(int id, MedicalRecordRequest request);

        /// <summary>
        /// Delete a medical record
        /// </summary>
        /// <param name="id">The ID of the medical record to delete</param>
        Task<bool> DeleteAsync(int id);
    }
} 