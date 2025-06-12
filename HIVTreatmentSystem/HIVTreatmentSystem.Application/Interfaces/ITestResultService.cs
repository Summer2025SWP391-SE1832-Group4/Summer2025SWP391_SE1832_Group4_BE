using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Services
{
    /// <summary>
    /// Service interface for managing test results
    /// </summary>
    public interface ITestResultService
    {
        /// <summary>
        /// Get all test results
        /// </summary>
        /// <returns>Collection of test result responses</returns>
        Task<IEnumerable<TestResultResponse>> GetAllAsync();

        /// <summary>
        /// Get a test result by its ID
        /// </summary>
        /// <param name="id">Test result ID</param>
        /// <returns>Test result response if found, null otherwise</returns>
        Task<TestResultResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Get test results by patient ID
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <returns>Collection of test result responses for the patient</returns>
        Task<IEnumerable<TestResultResponse>> GetByPatientIdAsync(int patientId);

        /// <summary>
        /// Get test results by medical record ID
        /// </summary>
        /// <param name="medicalRecordId">Medical record ID</param>
        /// <returns>Collection of test result responses for the medical record</returns>
        Task<IEnumerable<TestResultResponse>> GetByMedicalRecordIdAsync(int medicalRecordId);

        /// <summary>
        /// Create a new test result
        /// </summary>
        /// <param name="request">Test result creation request</param>
        /// <returns>Created test result response</returns>
        Task<TestResultResponse> CreateAsync(TestResultRequest request);

        /// <summary>
        /// Update an existing test result
        /// </summary>
        /// <param name="id">Test result ID</param>
        /// <param name="request">Test result update request</param>
        /// <returns>Updated test result response</returns>
        Task<TestResultResponse> UpdateAsync(int id, TestResultRequest request);

        /// <summary>
        /// Delete a test result
        /// </summary>
        /// <param name="id">Test result ID to delete</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        Task<bool> DeleteAsync(int id);
    }
} 