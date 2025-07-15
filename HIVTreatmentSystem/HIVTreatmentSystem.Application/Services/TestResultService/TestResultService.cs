using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Repositories;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services
{
    public class TestResultService : ITestResultService
    {
        private readonly ITestResultRepository _repository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for TestResultService
        /// </summary>
        /// <param name="repository">Test result repository</param>
        /// <param name="appointmentRepository">Appointment repository</param>
        /// <param name="mapper">AutoMapper instance</param>
        public TestResultService(
            ITestResultRepository repository,
            IAppointmentRepository appointmentRepository,
            IMapper mapper)
        {
            _repository = repository;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TestResultResponse>> GetAllAsync()
        {
            var testResults = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TestResultResponse>>(testResults);
        }

        /// <summary>
        /// Get all test results filtered by today's date
        /// </summary>
        /// <returns>Collection of test results from today</returns>
        public async Task<IEnumerable<TestResultResponse>> GetAllTodayAsync()
        {
            var testResults = await _repository.GetAllAsync();
            var todayResults = testResults.Where(tr => tr.TestDate.Date == DateTime.Today);
            return _mapper.Map<IEnumerable<TestResultResponse>>(todayResults);
        }

        /// <inheritdoc/>
        public async Task<TestResultResponse?> GetByIdAsync(int id)
        {
            var testResult = await _repository.GetByIdAsync(id);
            return testResult != null ? _mapper.Map<TestResultResponse>(testResult) : null;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TestResultResponse>> GetByPatientIdAsync(int patientId)
        {
            var testResults = await _repository.GetByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<TestResultResponse>>(testResults);
        }

        /// <summary>
        /// Get test results by patient ID filtered by today's date
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <returns>Collection of test results for the patient from today</returns>
        public async Task<IEnumerable<TestResultResponse>> GetByPatientIdTodayAsync(int patientId)
        {
            var testResults = await _repository.GetByPatientIdAsync(patientId);
            var todayResults = testResults.Where(tr => tr.TestDate.Date == DateTime.Today);
            return _mapper.Map<IEnumerable<TestResultResponse>>(todayResults);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TestResultResponse>> GetByMedicalRecordIdAsync(int medicalRecordId)
        {
            var testResults = await _repository.GetByMedicalRecordIdAsync(medicalRecordId);
            return _mapper.Map<IEnumerable<TestResultResponse>>(testResults);
        }

        /// <summary>
        /// Get test results by medical record ID filtered by today's date
        /// </summary>
        /// <param name="medicalRecordId">Medical record ID</param>
        /// <returns>Collection of test results for the medical record from today</returns>
        public async Task<IEnumerable<TestResultResponse>> GetByMedicalRecordIdTodayAsync(int medicalRecordId)
        {
            var testResults = await _repository.GetByMedicalRecordIdAsync(medicalRecordId);
            var todayResults = testResults.Where(tr => tr.TestDate.Date == DateTime.Today);
            return _mapper.Map<IEnumerable<TestResultResponse>>(todayResults);
        }

        /// <inheritdoc/>
        public async Task<TestResultListResponse> GetByAppointmentIdAsync(int appointmentId)
        {
            var testResults = await _repository.GetByAppointmentIdAsync(appointmentId);
            var mappedResults = _mapper.Map<IEnumerable<TestResultResponse>>(testResults);
            
            return new TestResultListResponse
            {
                TestResults = mappedResults,
                TotalCount = mappedResults.Count(),
                AppointmentId = appointmentId
            };
        }

        /// <summary>
        /// Get single test result by appointment ID
        /// Returns the first test result for the appointment, or null if none found
        /// </summary>
        /// <param name="appointmentId">Appointment ID</param>
        /// <returns>Single test result for the appointment, or null if not found</returns>
        public async Task<TestResultResponse?> GetSingleByAppointmentIdAsync(int appointmentId)
        {
            var testResults = await _repository.GetByAppointmentIdAsync(appointmentId);
            var firstResult = testResults.FirstOrDefault();
            return firstResult != null ? _mapper.Map<TestResultResponse>(firstResult) : null;
        }

        /// <summary>
        /// Get test results by appointment ID filtered by today's date
        /// </summary>
        /// <param name="appointmentId">Appointment ID</param>
        /// <returns>Test result list response for the appointment from today</returns>
        public async Task<TestResultListResponse> GetByAppointmentIdTodayAsync(int appointmentId)
        {
            var testResults = await _repository.GetByAppointmentIdAsync(appointmentId);
            var todayResults = testResults.Where(tr => tr.TestDate.Date == DateTime.Today);
            var mappedResults = _mapper.Map<IEnumerable<TestResultResponse>>(todayResults);
            
            return new TestResultListResponse
            {
                TestResults = mappedResults,
                TotalCount = mappedResults.Count(),
                AppointmentId = appointmentId
            };
        }

        /// <inheritdoc/>
        public async Task<TestResultResponse> CreateAsync(TestResultRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // if (request.TestDate > DateTime.UtcNow)
            //     throw new ArgumentException("Test date cannot be in the future");

            // Appointment is required (validated by DTO)
            var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(request.AppointmentId);
            if (appointment == null)
                throw new ArgumentException($"Appointment with ID {request.AppointmentId} not found");
            
            // Map request to entity
            var testResult = _mapper.Map<TestResult>(request);
            testResult.PatientId = appointment.PatientId;
            testResult.TestDate = DateTime.UtcNow;
            // Add to repository
            var createdTestResult = await _repository.AddAsync(testResult);

            // Map and return response
            return _mapper.Map<TestResultResponse>(createdTestResult);
        }

        public async Task<TestResultResponse> UpdateAsync(int id, TestResultRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // if (request.TestDate > DateTime.UtcNow)
            //     throw new ArgumentException("Test date cannot be in the future");

            var existingTestResult = await _repository.GetByIdAsync(id);
            if (existingTestResult == null)
                throw new KeyNotFoundException($"Test result with ID {id} not found");
            existingTestResult.TestDate = DateTime.UtcNow;
            // Resolve patient via appointment
            var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(request.AppointmentId);
            if (appointment == null)
                throw new ArgumentException($"Appointment with ID {request.AppointmentId} not found");

            _mapper.Map(request, existingTestResult);
            existingTestResult.PatientId = appointment.PatientId;

            var updated = await _repository.UpdateAsync(existingTestResult);
            return _mapper.Map<TestResultResponse>(updated);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
} 