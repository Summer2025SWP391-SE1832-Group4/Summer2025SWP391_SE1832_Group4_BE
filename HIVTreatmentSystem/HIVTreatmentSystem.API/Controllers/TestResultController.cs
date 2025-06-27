using HIVTreatmentSystem.API.Models;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    /// <summary>
    /// Controller for managing test results
    /// </summary>
    [ApiController]
    [Route("api/test-result")]
    // [Authorize]
    public class TestResultController : ControllerBase
    {
        private readonly ITestResultService _testResultService;

        /// <summary>
        /// Constructor for TestResultController
        /// </summary>
        /// <param name="testResultService">Test result service</param>
        public TestResultController(ITestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        /// <summary>
        /// Get all test results
        /// </summary>
        /// <returns>List of test results</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TestResultResponse>>>> GetAll()
        {
            var testResults = await _testResultService.GetAllAsync();
            if (testResults == null || !testResults.Any())
                return NotFound(new ApiResponse("No test results found."));
            return Ok(new ApiResponse("Success", testResults));
        }

        /// <summary>
        /// Get a test result by ID
        /// </summary>
        /// <param name="id">Test result ID</param>
        /// <returns>Test result details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TestResultResponse>>> GetById(int id)
        {
            var testResult = await _testResultService.GetByIdAsync(id);
            if (testResult == null)
                return NotFound(new ApiResponse("Test result not found."));

            return Ok(new ApiResponse("Success", testResult));
        }

        /// <summary>
        /// Get test result by appointment ID
        /// Returns a single test result for the appointment
        /// </summary>
        /// <param name="appointmentId">Appointment ID</param>
        /// <returns>Single test result for the appointment</returns>
        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<ApiResponse<TestResultResponse>>> GetByAppointmentId(int appointmentId)
        {
            var testResult = await _testResultService.GetSingleByAppointmentIdAsync(appointmentId);
            if (testResult == null)
                return NotFound(new ApiResponse("No test result found for the specified appointment."));
            return Ok(new ApiResponse("Success", testResult));
        }

        /// <summary>
        /// Get all test results by appointment ID
        /// Returns all test results for the appointment as a list
        /// </summary>
        /// <param name="appointmentId">Appointment ID</param>
        /// <returns>List of test results for the appointment</returns>
        [HttpGet("appointment/{appointmentId}/list")]
        public async Task<ActionResult<ApiResponse<TestResultListResponse>>> GetAllByAppointmentId(int appointmentId)
        {
            var testResults = await _testResultService.GetByAppointmentIdAsync(appointmentId);
            if (testResults.TotalCount == 0)
                return NotFound(new ApiResponse("No test results found for the specified appointment."));
            return Ok(new ApiResponse("Success", testResults));
        }

        /// <summary>
        /// Get all test results by patient ID
        /// Returns all test results for the specified patient
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <returns>List of test results for the patient</returns>
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TestResultResponse>>>> GetByPatientId(int patientId)
        {
            var testResults = await _testResultService.GetByPatientIdAsync(patientId);
            if (testResults == null || !testResults.Any())
                return NotFound(new ApiResponse("No test results found for the specified patient."));
            return Ok(new ApiResponse("Success", testResults));
        }

        /// <summary>
        /// Create a new test result
        /// </summary>
        /// <param name="request">Test result creation request</param>
        /// <returns>Created test result</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TestResultResponse>>> Create(TestResultRequest request)
        {
            var testResult = await _testResultService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = testResult.TestResultId }, 
                new ApiResponse("Test result created successfully", testResult));
        }

        /// <summary>
        /// Update an existing test result
        /// </summary>
        /// <param name="id">Test result ID</param>
        /// <param name="request">Test result update request</param>
        /// <returns>Updated test result</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TestResultResponse>>> Update(int id, TestResultRequest request)
        {
            try
            {
                var testResult = await _testResultService.UpdateAsync(id, request);
                return Ok(new ApiResponse("Test result updated successfully", testResult));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse("Test result not found."));
            }
        }

        /// <summary>
        /// Delete a test result
        /// </summary>
        /// <param name="id">Test result ID</param>
        /// <returns>Success message</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            var result = await _testResultService.DeleteAsync(id);
            if (!result)
                return NotFound(new ApiResponse("Test result not found."));

            return Ok(new ApiResponse("Test result deleted successfully."));
        }
    }
} 