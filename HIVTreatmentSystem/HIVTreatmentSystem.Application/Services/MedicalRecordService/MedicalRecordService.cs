using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
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
        private readonly IMapper _mapper;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository,
            IAppointmentRepository appointmentRepository,
            IMapper mapper)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _appointmentRepository = appointmentRepository;
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
            // Resolve PatientId & DoctorId from Appointment if not provided
            var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(request.AppointmentId);
            if (appointment == null)
                throw new ArgumentException($"Appointment with ID {request.AppointmentId} not found.");

            // Check if patient already has a medical record (1-to-1 constraint)
            var existingRecord = await _medicalRecordRepository.HasMedicalRecordAsync(appointment.PatientId);
            if (existingRecord)
                throw new InvalidOperationException($"Patient with ID {appointment.PatientId} already has a medical record. Use update instead.");

            var medicalRecord = _mapper.Map<MedicalRecord>(request);
            medicalRecord.PatientId = appointment.PatientId;
            medicalRecord.DoctorId = appointment.DoctorId;
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
            // Ensure PatientId / DoctorId remain in sync with original appointment (immutable)
            // Only update other fields
            // (If PatientId/DoctorId were provided in request, they are ignored.)
            var existingAppointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(existingMedicalRecord.AppointmentId);
            if (existingAppointment != null)
            {
                existingMedicalRecord.PatientId = existingAppointment.PatientId;
                existingMedicalRecord.DoctorId = existingAppointment.DoctorId;
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
                
                // Ensure PatientId / DoctorId remain in sync with original appointment
                var existingAppointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(existingRecord.AppointmentId);
                if (existingAppointment != null)
                {
                    existingRecord.PatientId = existingAppointment.PatientId;
                    existingRecord.DoctorId = existingAppointment.DoctorId;
                }
                
                await _medicalRecordRepository.UpdateAsync(existingRecord);
                return _mapper.Map<MedicalRecordResponse>(existingRecord);
            }
            else
            {
                // Create new record
                var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(request.AppointmentId);
                if (appointment == null)
                    throw new ArgumentException($"Appointment with ID {request.AppointmentId} not found.");

                if (appointment.PatientId != patientId)
                    throw new ArgumentException($"Appointment does not belong to patient with ID {patientId}.");

                var medicalRecord = _mapper.Map<MedicalRecord>(request);
                medicalRecord.PatientId = patientId;
                medicalRecord.DoctorId = appointment.DoctorId;
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

            // If AppointmentId is provided, validate it belongs to the patient
            if (request.AppointmentId.HasValue)
            {
                var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(request.AppointmentId.Value);
                if (appointment == null)
                    throw new ArgumentException($"Appointment with ID {request.AppointmentId} not found.");
                
                if (appointment.PatientId != request.PatientId)
                    throw new ArgumentException($"Appointment with ID {request.AppointmentId} does not belong to patient with ID {request.PatientId}.");
                
                if (appointment.DoctorId != request.DoctorId)
                    throw new ArgumentException($"Appointment with ID {request.AppointmentId} is not assigned to doctor with ID {request.DoctorId}.");
            }

            // Create medical record
            var medicalRecord = new MedicalRecord
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                AppointmentId = request.AppointmentId ?? 0, // Use 0 if no specific appointment
                ConsultationDate = request.ConsultationDate,
                Symptoms = request.Symptoms,
                Diagnosis = request.Diagnosis,
                DoctorNotes = request.DoctorNotes,
                NextSteps = request.NextSteps,
                CoinfectionDiseases = request.CoinfectionDiseases,
                DrugAllergyHistory = request.DrugAllergyHistory
            };

            var createdMedicalRecord = await _medicalRecordRepository.CreateAsync(medicalRecord);
            return _mapper.Map<MedicalRecordResponse>(createdMedicalRecord);
        }
    }
} 