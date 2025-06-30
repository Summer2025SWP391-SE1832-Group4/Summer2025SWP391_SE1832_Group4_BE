using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    /// <summary>
    /// Request model for adding test result to medical record
    /// Payload chỉ cần TestResultId
    /// </summary>
    public class AddTestResultToMedicalRecordRequest
    {
        /// <summary>
        /// ID của test result cần add vào medical record
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TestResultId must be greater than 0")]
        public int TestResultId { get; set; }
    }
} 