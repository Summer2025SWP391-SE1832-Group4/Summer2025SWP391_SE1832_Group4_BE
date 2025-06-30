using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Repositories;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services
{
    /// <summary>
    /// Service implementation for Medical Record operations
    /// Mỗi Patient chỉ có một MedicalRecord duy nhất (1-to-1 relationship)
    /// </summary>
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ITestResultRepository _testResultRepository;
        private readonly IMapper _mapper;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository,
            IAppointmentRepository appointmentRepository,
            ITestResultRepository testResultRepository,
            IMapper mapper)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _appointmentRepository = appointmentRepository;
            _testResultRepository = testResultRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MedicalRecordResponse>> GetAllAsync()
        {
            var medicalRecords = await _medicalRecordRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MedicalRecordResponse>>(medicalRecords);
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponse?> GetByIdAsync(int id)
        {
            var medicalRecord = await _medicalRecordRepository.GetByIdAsync(id);
            return medicalRecord == null ? null : _mapper.Map<MedicalRecordResponse>(medicalRecord);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MedicalRecordResponse>> GetByPatientIdAsync(int patientId)
        {
            var medicalRecords = await _medicalRecordRepository.GetByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<MedicalRecordResponse>>(medicalRecords);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MedicalRecordResponse>> GetByDoctorIdAsync(int doctorId)
        {
            var medicalRecords = await _medicalRecordRepository.GetByDoctorIdAsync(doctorId);
            return _mapper.Map<IEnumerable<MedicalRecordResponse>>(medicalRecords);
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponse> CreateAsync(MedicalRecordRequest request)
        {
            // Check if patient already has a medical record (1-to-1 constraint)
            var existingRecord = await _medicalRecordRepository.HasMedicalRecordAsync(request.PatientId);
            if (existingRecord)
                throw new InvalidOperationException($"Patient with ID {request.PatientId} already has a medical record. Use update instead.");

            var medicalRecord = _mapper.Map<MedicalRecord>(request);
            var createdMedicalRecord = await _medicalRecordRepository.CreateAsync(medicalRecord);

            return _mapper.Map<MedicalRecordResponse>(createdMedicalRecord);
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponse> UpdateAsync(int id, MedicalRecordRequest request)
        {
            var existingMedicalRecord = await _medicalRecordRepository.GetByIdAsync(id);
            if (existingMedicalRecord == null)
                throw new ArgumentException($"Medical record with ID {id} not found.");

            _mapper.Map(request, existingMedicalRecord);
            await _medicalRecordRepository.UpdateAsync(existingMedicalRecord);
            return _mapper.Map<MedicalRecordResponse>(existingMedicalRecord);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            var medicalRecord = await _medicalRecordRepository.GetByIdAsync(id);
            if (medicalRecord == null)
                throw new ArgumentException($"Medical record with ID {id} not found.");

            return await _medicalRecordRepository.DeleteAsync(medicalRecord);
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponse?> GetUniqueByPatientIdAsync(int patientId)
        {
            var medicalRecord = await _medicalRecordRepository.GetByPatientIdUniqueAsync(patientId);
            return medicalRecord == null ? null : _mapper.Map<MedicalRecordResponse>(medicalRecord);
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponse> CreateOrUpdateByPatientIdAsync(int patientId, MedicalRecordRequest request)
        {
            // Ensure request is for the correct patient
            if (request.PatientId != patientId)
                throw new ArgumentException($"Request PatientId ({request.PatientId}) does not match the provided patientId ({patientId}).");

            // Check if patient already has a medical record
            var existingRecord = await _medicalRecordRepository.GetByPatientIdUniqueAsync(patientId);

            if (existingRecord != null)
            {
                // Update existing record
                _mapper.Map(request, existingRecord);
                await _medicalRecordRepository.UpdateAsync(existingRecord);
                return _mapper.Map<MedicalRecordResponse>(existingRecord);
            }
            else
            {
                // Create new record
                var medicalRecord = _mapper.Map<MedicalRecord>(request);
                var createdMedicalRecord = await _medicalRecordRepository.CreateAsync(medicalRecord);
                return _mapper.Map<MedicalRecordResponse>(createdMedicalRecord);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> PatientHasMedicalRecordAsync(int patientId)
        {
            return await _medicalRecordRepository.HasMedicalRecordAsync(patientId);
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponse> CreateByPatientIdAsync(MedicalRecordByPatientRequest request)
        {
            // Check if patient already has a medical record (1-to-1 constraint)
            var existingRecord = await _medicalRecordRepository.HasMedicalRecordAsync(request.PatientId);
            if (existingRecord)
                throw new InvalidOperationException($"Patient with ID {request.PatientId} already has a medical record. Use update instead.");

            // Convert to standard request and create
            var medicalRecordRequest = new MedicalRecordRequest
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                ConsultationDate = request.ConsultationDate,
                Symptoms = request.Symptoms,
                Diagnosis = request.Diagnosis,
                PregnancyStatus = request.PregnancyStatus,
                PregnancyWeek = request.PregnancyWeek,
                DoctorNotes = request.DoctorNotes,
                NextSteps = request.NextSteps,
                UnderlyingDisease = request.UnderlyingDisease,
                DrugAllergyHistory = request.DrugAllergyHistory
            };

            return await CreateAsync(medicalRecordRequest);
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponse> CreateFromTestResultAsync(MedicalRecordFromTestResultRequest request)
        {
            // Validate test result exists
            var testResult = await _testResultRepository.GetByIdAsync(request.TestResultId);
            if (testResult == null)
                throw new ArgumentException($"Test result with ID {request.TestResultId} not found.");

            // Check if patient already has a medical record (1-to-1 constraint)
            var existingRecord = await _medicalRecordRepository.HasMedicalRecordAsync(testResult.PatientId);
            if (existingRecord)
                throw new InvalidOperationException($"Patient with ID {testResult.PatientId} already has a medical record. Use update instead.");

            // Create medical record based on test result
            var medicalRecord = new MedicalRecord
            {
                PatientId = testResult.PatientId,
                DoctorId = request.DoctorId,
                ConsultationDate = request.ConsultationDate,
                Symptoms = request.Symptoms,
                Diagnosis = request.Diagnosis,
                PregnancyStatus = request.PregnancyStatus,
                PregnancyWeek = request.PregnancyWeek,
                DoctorNotes = request.DoctorNotes,
                NextSteps = request.NextSteps,
                UnderlyingDisease = request.UnderlyingDisease,
                DrugAllergyHistory = request.DrugAllergyHistory
            };

            var createdMedicalRecord = await _medicalRecordRepository.CreateAsync(medicalRecord);

            // Update test result to link with the medical record
            testResult.MedicalRecordId = createdMedicalRecord.MedicalRecordId;
            await _testResultRepository.UpdateAsync(testResult);

            return _mapper.Map<MedicalRecordResponse>(createdMedicalRecord);
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponse> AddTestResultToMedicalRecordAsync(int medicalRecordId, AddTestResultToMedicalRecordRequest request)
        {
            // Validate medical record exists
            var medicalRecord = await _medicalRecordRepository.GetByIdAsync(medicalRecordId);
            if (medicalRecord == null)
                throw new ArgumentException($"Medical record with ID {medicalRecordId} not found.");

            // Validate test result exists
            var testResult = await _testResultRepository.GetByIdAsync(request.TestResultId);
            if (testResult == null)
                throw new ArgumentException($"Test result with ID {request.TestResultId} not found.");

            // Check if test result already linked to another medical record
            if (testResult.MedicalRecordId.HasValue && testResult.MedicalRecordId != medicalRecordId)
                throw new InvalidOperationException($"Test result with ID {request.TestResultId} is already linked to medical record {testResult.MedicalRecordId}.");

            // Check if test result belongs to the same patient as medical record
            if (testResult.PatientId != medicalRecord.PatientId)
                throw new InvalidOperationException($"Test result patient ID ({testResult.PatientId}) does not match medical record patient ID ({medicalRecord.PatientId}).");

            // Link test result to medical record
            testResult.MedicalRecordId = medicalRecordId;
            await _testResultRepository.UpdateAsync(testResult);

            // Return updated medical record with all test results
            var updatedMedicalRecord = await _medicalRecordRepository.GetByIdAsync(medicalRecordId);
            return _mapper.Map<MedicalRecordResponse>(updatedMedicalRecord);
        }
    }
} 