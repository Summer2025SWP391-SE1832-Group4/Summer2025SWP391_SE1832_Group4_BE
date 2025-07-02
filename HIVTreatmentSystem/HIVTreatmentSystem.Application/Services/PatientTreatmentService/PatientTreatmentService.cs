using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Repositories;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using System.Linq;

namespace HIVTreatmentSystem.Application.Services.PatientTreatmentService
{
    /// <summary>
    /// Service implementation for managing patient treatments
    /// </summary>
    public class PatientTreatmentService : IPatientTreatmentService
    {
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ITestResultRepository _testResultRepository;
        private readonly IMapper _mapper;

        public PatientTreatmentService(
            ITreatmentRepository treatmentRepository,
            IAppointmentRepository appointmentRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            ITestResultRepository testResultRepository,
            IMapper mapper)
        {
            _treatmentRepository = treatmentRepository;
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _testResultRepository = testResultRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientTreatmentResponse>> GetAllAsync()
        {
            var treatments = await _treatmentRepository.GetAllAsync();
            var responses = _mapper.Map<IEnumerable<PatientTreatmentResponse>>(treatments);
            
            // Ensure all related data is populated
            foreach (var response in responses)
            {
                await PopulateAdditionalDataAsync(response);
            }
            
            return responses;
        }

        public async Task<PatientTreatmentResponse?> GetByIdAsync(int id)
        {
            var treatment = await _treatmentRepository.GetByIdAsync(id);
            if (treatment == null) return null;
            
            var response = _mapper.Map<PatientTreatmentResponse>(treatment);
            await PopulateAdditionalDataAsync(response);
            
            return response;
        }

        public async Task<IEnumerable<PatientTreatmentResponse>> GetByPatientIdAsync(int patientId)
        {
            // Validate that patient exists
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
            {
                throw new ArgumentException($"Patient with ID {patientId} does not exist");
            }

            var treatments = await _treatmentRepository.GetTreatmentsByPatientAsync(patientId);
            var responses = _mapper.Map<IEnumerable<PatientTreatmentResponse>>(treatments);
            
            // Populate additional data for each response
            foreach (var response in responses)
            {
                await PopulateAdditionalDataAsync(response);
            }
            
            return responses;
        }

        public async Task<IEnumerable<PatientTreatmentResponse>> GetByDoctorIdAsync(int doctorId)
        {
            // Validate that doctor exists
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                throw new ArgumentException($"Doctor with ID {doctorId} does not exist");
            }

            var treatments = await _treatmentRepository.GetTreatmentsByDoctorAsync(doctorId);
            var responses = _mapper.Map<IEnumerable<PatientTreatmentResponse>>(treatments);
            
            // Populate additional data for each response
            foreach (var response in responses)
            {
                await PopulateAdditionalDataAsync(response);
            }
            
            return responses;
        }

        public async Task<PatientTreatmentResponse> CreateAsync(PatientTreatmentRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Validate required fields
            if (request.AppointmentId <= 0)
                throw new ArgumentException("Valid AppointmentId is required");

            if (request.RegimenId <= 0)
                throw new ArgumentException("Valid RegimenId is required");

            // Fetch appointment to derive patient & doctor
            var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(request.AppointmentId);
            if (appointment == null)
                throw new ArgumentException($"Appointment with ID {request.AppointmentId} not found");

            // Validate patient exists
            var patient = await _patientRepository.GetByIdAsync(appointment.PatientId);
            if (patient == null)
                throw new ArgumentException($"Patient with ID {appointment.PatientId} not found");

            // Validate doctor exists
            var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId);
            if (doctor == null)
                throw new ArgumentException($"Doctor with ID {appointment.DoctorId} not found");

            var treatment = _mapper.Map<PatientTreatment>(request);
            treatment.PatientId = appointment.PatientId;
            treatment.PrescribingDoctorId = appointment.DoctorId;
            treatment.StartDate = request.StartDate ?? DateTime.UtcNow;

            // Parse status string if provided
            if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<TreatmentStatus>(request.Status, true, out var status))
            {
                treatment.Status = status;
            }

            treatment = await _treatmentRepository.AddAsync(treatment);

            // Optional: Update medical record NextSteps
            await UpdateMedicalRecordNextStepsAsync(appointment.AppointmentId, $"Started regimen ID {treatment.RegimenId}");

            // Get the full treatment with all related data
            var fullTreatment = await _treatmentRepository.GetByIdAsync(treatment.PatientTreatmentId);
            var response = _mapper.Map<PatientTreatmentResponse>(fullTreatment);
            await PopulateAdditionalDataAsync(response);
            
            return response;
        }

        public async Task<PatientTreatmentResponse> UpdateAsync(int id, PatientTreatmentRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var existing = await _treatmentRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Treatment with ID {id} not found");

            _mapper.Map(request, existing);

            // Keep patient & doctor fixed via appointment if provided
            if (request.AppointmentId != 0)
            {
                var appt = await _appointmentRepository.GetAppointmentWithDetailsAsync(request.AppointmentId);
                if (appt != null)
                {
                    existing.PatientId = appt.PatientId;
                    existing.PrescribingDoctorId = appt.DoctorId;
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<TreatmentStatus>(request.Status, true, out var status))
            {
                existing.Status = status;
            }

            existing = await _treatmentRepository.UpdateAsync(existing);

            if (existing.Status == TreatmentStatus.Completed)
            {
                await UpdateMedicalRecordNextStepsAsync(existing.PatientTreatmentId, "Treatment completed");
            }

            // Get the full treatment with all related data
            var fullTreatment = await _treatmentRepository.GetByIdAsync(existing.PatientTreatmentId);
            var response = _mapper.Map<PatientTreatmentResponse>(fullTreatment);
            await PopulateAdditionalDataAsync(response);
            
            return response;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _treatmentRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Populate additional TestResult and MedicalRecord data for the response
        /// </summary>
        private async Task PopulateAdditionalDataAsync(PatientTreatmentResponse response)
        {
            if (response?.PatientId > 0)
            {
                // Get TestResults for the patient if not already populated
                if (response.PatientTestResults == null || !response.PatientTestResults.Any())
                {
                    var testResults = await _testResultRepository.GetByPatientIdAsync(response.PatientId);
                    response.PatientTestResults = _mapper.Map<ICollection<TestResultResponse>>(testResults);
                }

                // Get MedicalRecord for the patient if not already populated
                if (response.PatientMedicalRecord == null)
                {
                    var medicalRecord = await _medicalRecordRepository.GetByPatientIdUniqueAsync(response.PatientId);
                    if (medicalRecord != null)
                    {
                        response.PatientMedicalRecord = _mapper.Map<MedicalRecordResponse>(medicalRecord);
                    }
                }
            }
        }

        private async Task UpdateMedicalRecordNextStepsAsync(int appointmentId, string note)
        {
            // Since MedicalRecord no longer has direct AppointmentId, 
            // we need to find it through TestResult that might be related to the appointment
            var allRecords = await _medicalRecordRepository.GetAllAsync();
            
            // Find medical record where any TestResult's AppointmentId matches
            var record = allRecords.FirstOrDefault(r => 
                r.TestResults != null && r.TestResults.Any(tr => tr.AppointmentId == appointmentId));
            
            if (record != null)
            {
                record.NextSteps = note;
                await _medicalRecordRepository.UpdateAsync(record);
            }
        }
    }
} 