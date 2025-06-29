using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
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
        private readonly IMapper _mapper;

        public PatientTreatmentService(
            ITreatmentRepository treatmentRepository,
            IAppointmentRepository appointmentRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IMapper mapper)
        {
            _treatmentRepository = treatmentRepository;
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientTreatmentResponse>> GetAllAsync()
        {
            var treatments = await _treatmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientTreatmentResponse>>(treatments);
        }

        public async Task<PatientTreatmentResponse?> GetByIdAsync(int id)
        {
            var treatment = await _treatmentRepository.GetByIdAsync(id);
            return treatment != null ? _mapper.Map<PatientTreatmentResponse>(treatment) : null;
        }

        public async Task<IEnumerable<PatientTreatmentResponse>> GetByPatientIdAsync(int patientId)
        {
            var treatments = await _treatmentRepository.GetTreatmentsByPatientAsync(patientId);
            return _mapper.Map<IEnumerable<PatientTreatmentResponse>>(treatments);
        }

        public async Task<IEnumerable<PatientTreatmentResponse>> GetByDoctorIdAsync(int doctorId)
        {
            var treatments = await _treatmentRepository.GetTreatmentsByDoctorAsync(doctorId);
            return _mapper.Map<IEnumerable<PatientTreatmentResponse>>(treatments);
        }

        public async Task<PatientTreatmentResponse> CreateAsync(PatientTreatmentRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Fetch appointment to derive patient & doctor
            var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(request.AppointmentId);
            if (appointment == null)
                throw new ArgumentException($"Appointment {request.AppointmentId} not found");

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

            return _mapper.Map<PatientTreatmentResponse>(treatment);
        }

        public async Task<PatientTreatmentResponse> UpdateAsync(int id, PatientTreatmentRequest request)
        {
            var existing = await _treatmentRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Treatment {id} not found");

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

            return _mapper.Map<PatientTreatmentResponse>(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _treatmentRepository.DeleteAsync(id);
        }

        private async Task UpdateMedicalRecordNextStepsAsync(int appointmentId, string note)
        {
            // Since MedicalRecord no longer has direct AppointmentId, 
            // we need to find it through TestResult that might be related to the appointment
            var allRecords = await _medicalRecordRepository.GetAllAsync();
            
            // Find medical record where TestResult's AppointmentId matches
            var record = allRecords.FirstOrDefault(r => 
                r.TestResult != null && r.TestResult.AppointmentId == appointmentId);
            
            if (record != null)
            {
                record.NextSteps = note;
                await _medicalRecordRepository.UpdateAsync(record);
            }
        }
    }
} 