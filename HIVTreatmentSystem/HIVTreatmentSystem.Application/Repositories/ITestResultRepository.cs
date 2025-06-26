using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Application.Repositories
{
    /// <summary>
    /// Repository interface for managing test results
    /// </summary>
    public interface ITestResultRepository
    {
        /// <summary>
        /// Get all test results
        /// </summary>
        /// <returns>Collection of test results</returns>
        Task<IEnumerable<TestResult>> GetAllAsync();

        /// <summary>
        /// Get a test result by its ID
        /// </summary>
        /// <param name="id">Test result ID</param>
        /// <returns>Test result if found, null otherwise</returns>
        Task<TestResult?> GetByIdAsync(int id);

        /// <summary>
        /// Get test results by patient ID
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <returns>Collection of test results for the patient</returns>
        Task<IEnumerable<TestResult>> GetByPatientIdAsync(int patientId);

        /// <summary>
        /// Get test results by medical record ID
        /// </summary>
        /// <param name="medicalRecordId">Medical record ID</param>
        /// <returns>Collection of test results for the medical record</returns>
        Task<IEnumerable<TestResult>> GetByMedicalRecordIdAsync(int medicalRecordId);

        /// <summary>
        /// Get test results by appointment ID
        /// Only returns test results for patients in appointments with CheckedIn status
        /// </summary>
        /// <param name="appointmentId">Appointment ID</param>
        /// <returns>Collection of test results for the patient in the CheckedIn appointment</returns>
        Task<IEnumerable<TestResult>> GetByAppointmentIdAsync(int appointmentId);

        /// <summary>
        /// Add a new test result
        /// </summary>
        /// <param name="testResult">Test result to add</param>
        /// <returns>Added test result</returns>
        Task<TestResult> AddAsync(TestResult testResult);

        /// <summary>
        /// Update an existing test result
        /// </summary>
        /// <param name="testResult">Test result to update</param>
        /// <returns>Updated test result</returns>
        Task<TestResult> UpdateAsync(TestResult testResult);

        /// <summary>
        /// Delete a test result
        /// </summary>
        /// <param name="id">Test result ID to delete</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        Task<bool> DeleteAsync(int id);
    }
} 