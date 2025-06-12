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
        private readonly IMapper _mapper;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository, IMapper mapper)
        {
            _medicalRecordRepository = medicalRecordRepository;
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
    }
} 