using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Repositories;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services
{
    /// <summary>
    /// Service implementation for managing test results
    /// </summary>
    public class TestResultService : ITestResultService
    {
        private readonly ITestResultRepository _repository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for TestResultService
        /// </summary>
        /// <param name="repository">Test result repository</param>
        /// <param name="appointmentRepository">Appointment repository</param>
        /// <param name="medicalRecordRepository">Medical record repository</param>
        /// <param name="mapper">AutoMapper instance</param>
        public TestResultService(
            ITestResultRepository repository,
            IAppointmentRepository appointmentRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IMapper mapper)
        {
            _repository = repository;
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TestResultResponse>> GetAllAsync()
        {
            var testResults = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TestResultResponse>>(testResults);
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

        /// <inheritdoc/>
        public async Task<IEnumerable<TestResultResponse>> GetByMedicalRecordIdAsync(int medicalRecordId)
        {
            var testResults = await _repository.GetByMedicalRecordIdAsync(medicalRecordId);
            return _mapper.Map<IEnumerable<TestResultResponse>>(testResults);
        }

        /// <inheritdoc/>
        public async Task<TestResultResponse> CreateAsync(TestResultRequest request)
        {
            // Validate request
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.TestDate > DateTime.UtcNow)
                throw new ArgumentException("Test date cannot be in the future");

            // Resolve PatientId if not provided
            int patientId;
            if (request.PatientId.HasValue && request.PatientId.Value > 0)
            {
                patientId = request.PatientId.Value;
            }
            else if (request.MedicalRecordId.HasValue)
            {
                var medicalRecord = await _medicalRecordRepository.GetByIdAsync(request.MedicalRecordId.Value);
                if (medicalRecord == null)
                    throw new ArgumentException($"Medical record with ID {request.MedicalRecordId.Value} not found");
                patientId = medicalRecord.PatientId;
            }
            else if (request.AppointmentId.HasValue)
            {
                var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(request.AppointmentId.Value);
                if (appointment == null)
                    throw new ArgumentException($"Appointment with ID {request.AppointmentId.Value} not found");
                patientId = appointment.PatientId;
            }
            else
            {
                throw new ArgumentException("PatientId, MedicalRecordId or AppointmentId must be provided");
            }

            // Map request to entity
            var testResult = _mapper.Map<TestResult>(request);
            testResult.PatientId = patientId;

            // Add to repository
            var createdTestResult = await _repository.AddAsync(testResult);

            // Map and return response
            return _mapper.Map<TestResultResponse>(createdTestResult);
        }

        /// <inheritdoc/>
        public async Task<TestResultResponse> UpdateAsync(int id, TestResultRequest request)
        {
            // Validate request
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.TestDate > DateTime.UtcNow)
                throw new ArgumentException("Test date cannot be in the future");

            // Get existing test result
            var existingTestResult = await _repository.GetByIdAsync(id);
            if (existingTestResult == null)
                throw new KeyNotFoundException($"Test result with ID {id} not found");

            // Preserve PatientId logic similar to Create
            int patientId = existingTestResult.PatientId;

            if (request.PatientId.HasValue && request.PatientId.Value > 0)
            {
                patientId = request.PatientId.Value;
            }
            else if (request.MedicalRecordId.HasValue)
            {
                var medicalRecord = await _medicalRecordRepository.GetByIdAsync(request.MedicalRecordId.Value);
                if (medicalRecord == null)
                    throw new ArgumentException($"Medical record with ID {request.MedicalRecordId.Value} not found");
                patientId = medicalRecord.PatientId;
            }
            else if (request.AppointmentId.HasValue)
            {
                var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(request.AppointmentId.Value);
                if (appointment == null)
                    throw new ArgumentException($"Appointment with ID {request.AppointmentId.Value} not found");
                patientId = appointment.PatientId;
            }

            _mapper.Map(request, existingTestResult);
            existingTestResult.PatientId = patientId;

            // Update in repository
            var updatedTestResult = await _repository.UpdateAsync(existingTestResult);

            // Map and return response
            return _mapper.Map<TestResultResponse>(updatedTestResult);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
} 