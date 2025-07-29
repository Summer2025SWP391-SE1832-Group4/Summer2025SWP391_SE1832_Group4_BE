using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for Medical Record operations
    /// </summary>
    public interface IMedicalRecordRepository
    {
        /// <summary>
        /// Get all medical records
        /// </summary>
        Task<IEnumerable<MedicalRecord>> GetAllAsync();

        /// <summary>
        /// Get a medical record by its ID
        /// </summary>
        /// <param name="id">The ID of the medical record to retrieve</param>
        Task<MedicalRecord?> GetByIdAsync(int id);

        /// <summary>
        /// Get medical records by patient ID
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId);

        /// <summary>
        /// Get medical records by doctor ID
        /// </summary>
        /// <param name="doctorId">The ID of the doctor</param>
        Task<IEnumerable<MedicalRecord>> GetByDoctorIdAsync(int doctorId);

        /// <summary>
        /// Create a new medical record
        /// </summary>
        /// <param name="medicalRecord">The medical record to create</param>
        Task<MedicalRecord> CreateAsync(MedicalRecord medicalRecord);

        /// <summary>
        /// Update an existing medical record
        /// </summary>
        /// <param name="medicalRecord">The medical record to update</param>
        Task<bool> UpdateAsync(MedicalRecord medicalRecord);

        /// <summary>
        /// Delete a medical record
        /// </summary>
        /// <param name="medicalRecord">The medical record to delete</param>
        Task<bool> DeleteAsync(MedicalRecord medicalRecord);

        /// <summary>
        /// Check if patient already has a medical record
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        Task<bool> HasMedicalRecordAsync(int patientId);
        
        
    }
} 