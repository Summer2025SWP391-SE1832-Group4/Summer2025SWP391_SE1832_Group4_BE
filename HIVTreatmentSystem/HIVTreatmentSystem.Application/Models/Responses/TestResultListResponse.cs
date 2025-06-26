using System.Collections.Generic;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    /// <summary>
    /// Response model for test results list
    /// </summary>
    public class TestResultListResponse
    {
        /// <summary>
        /// List of test results
        /// </summary>
        public IEnumerable<TestResultResponse> TestResults { get; set; } = new List<TestResultResponse>();

        /// <summary>
        /// Total count of test results
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Appointment ID associated with these test results
        /// </summary>
        public int AppointmentId { get; set; }
    }
} 