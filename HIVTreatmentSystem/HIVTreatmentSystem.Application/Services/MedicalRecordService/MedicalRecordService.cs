using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Repositories;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services
{
    /// <summary>
    /// Service implementation for Medical Record operations
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
            // Validate test result exists
            var testResult = await _testResultRepository.GetByIdAsync(request.TestResultId);
            if (testResult == null)
                throw new ArgumentException($"Test result with ID {request.TestResultId} not found.");

            // Check if test result already has a medical record
            if (testResult.MedicalRecordId.HasValue)
                throw new InvalidOperationException($"Test result with ID {request.TestResultId} already has a medical record.");

            // Check if patient already has a medical record (1-to-1 constraint)
            var existingRecord = await _medicalRecordRepository.HasMedicalRecordAsync(testResult.PatientId);
            if (existingRecord)
                throw new InvalidOperationException($"Patient with ID {testResult.PatientId} already has a medical record. Use update instead.");

            var medicalRecord = _mapper.Map<MedicalRecord>(request);
            medicalRecord.PatientId = testResult.PatientId; // PatientId comes from TestResult
            var createdMedicalRecord = await _medicalRecordRepository.CreateAsync(medicalRecord);

            // Update test result to link with medical record
            testResult.MedicalRecordId = createdMedicalRecord.MedicalRecordId;
            await _testResultRepository.UpdateAsync(testResult);

            return _mapper.Map<MedicalRecordResponse>(createdMedicalRecord);
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponse> UpdateAsync(int id, MedicalRecordRequest request)
        {
            var existingMedicalRecord = await _medicalRecordRepository.GetByIdAsync(id);
            if (existingMedicalRecord == null)
                throw new ArgumentException($"Medical record with ID {id} not found.");

            _mapper.Map(request, existingMedicalRecord);
            // For medical records created from test results, validate patient and doctor remain consistent
            // PatientId comes from TestResult, DoctorId should remain same or be updated carefully
            var testResult = await _testResultRepository.GetByIdAsync(existingMedicalRecord.TestResultId);
            if (testResult != null)
            {
                existingMedicalRecord.PatientId = testResult.PatientId;
                // DoctorId can be updated if needed
            }
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
            // Check if patient already has a medical record
            var existingRecord = await _medicalRecordRepository.GetByPatientIdUniqueAsync(patientId);

            if (existingRecord != null)
            {
                // Update existing record
                _mapper.Map(request, existingRecord);
                
                // For medical records created from test results, ensure PatientId remains consistent
                var testResult = await _testResultRepository.GetByIdAsync(existingRecord.TestResultId);
                if (testResult != null)
                {
                    existingRecord.PatientId = testResult.PatientId;
                    // DoctorId can be updated
                }
                
                await _medicalRecordRepository.UpdateAsync(existingRecord);
                return _mapper.Map<MedicalRecordResponse>(existingRecord);
            }
            else
            {
                // Create new record
                var testResult = await _testResultRepository.GetByIdAsync(request.TestResultId);
                if (testResult == null)
                    throw new ArgumentException($"Test result with ID {request.TestResultId} not found.");

                if (testResult.PatientId != patientId)
                    throw new ArgumentException($"Test result does not belong to patient with ID {patientId}.");

                var medicalRecord = _mapper.Map<MedicalRecord>(request);
                medicalRecord.PatientId = patientId;
                var createdMedicalRecord = await _medicalRecordRepository.CreateAsync(medicalRecord);

                // Update test result to link with medical record
                testResult.MedicalRecordId = createdMedicalRecord.MedicalRecordId;
                await _testResultRepository.UpdateAsync(testResult);

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

            // This method is deprecated in favor of CreateFromTestResultAsync
            // But keeping for backward compatibility
            throw new NotSupportedException("Creating medical records by patient ID is deprecated. Use CreateFromTestResultAsync instead.");
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponse> CreateFromTestResultAsync(MedicalRecordFromTestResultRequest request)
        {
            // Validate test result exists
            var testResult = await _testResultRepository.GetByIdAsync(request.TestResultId);
            if (testResult == null)
                throw new ArgumentException($"Test result with ID {request.TestResultId} not found.");

            // Check if test result already has a medical record
            if (testResult.MedicalRecordId.HasValue)
                throw new InvalidOperationException($"Test result with ID {request.TestResultId} already has a medical record.");

            // Check if patient already has a medical record (1-to-1 constraint)
            var existingRecord = await _medicalRecordRepository.HasMedicalRecordAsync(testResult.PatientId);
            if (existingRecord)
                throw new InvalidOperationException($"Patient with ID {testResult.PatientId} already has a medical record. Use update instead.");

            // Create medical record based on test result
            var medicalRecord = new MedicalRecord
            {
                TestResultId = request.TestResultId,
                PatientId = testResult.PatientId,
                DoctorId = request.DoctorId,
                ConsultationDate = request.ConsultationDate,
                Symptoms = request.Symptoms,
                Diagnosis = request.Diagnosis,
                DoctorNotes = request.DoctorNotes,
                NextSteps = request.NextSteps,
                CoinfectionDiseases = request.CoinfectionDiseases,
                DrugAllergyHistory = request.DrugAllergyHistory
            };

            var createdMedicalRecord = await _medicalRecordRepository.CreateAsync(medicalRecord);

            // Update test result to link with medical record
            testResult.MedicalRecordId = createdMedicalRecord.MedicalRecordId;
            await _testResultRepository.UpdateAsync(testResult);

            return _mapper.Map<MedicalRecordResponse>(createdMedicalRecord);
        }
    }
} 