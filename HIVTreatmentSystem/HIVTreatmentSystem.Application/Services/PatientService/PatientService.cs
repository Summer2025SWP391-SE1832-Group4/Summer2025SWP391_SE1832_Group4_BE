using AutoMapper;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Pages;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Services.PatientService
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<PageResult<PatientResponse>> GetAllPatientsAsync(
            int? accountId,
            DateTime? dateOfBirth,
            Gender? gender,
            string? address,
            DateTime? hivDiagnosisDate,
            string? consentInformation,
            bool isDescending = false,
            string? sortBy = "",
            int pageIndex = 1,
            int pageSize = 10)
        {
            var patients = await _patientRepository.GetAllPatientsAsync(
                accountId,
                dateOfBirth,
                gender,
                address,
                hivDiagnosisDate,
                consentInformation,
                isDescending,
                sortBy);

            var paged = patients.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var dtoList = _mapper.Map<List<PatientResponse>>(paged);

            return new PageResult<PatientResponse>(
                dtoList,
                pageSize,
                pageIndex,
                patients.Count()
            );
        }

        public async Task<bool> UpdatePatientAsync(int patientId, UpdatePatientRequest dto)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null) return false;

            if (dto.DateOfBirth != patient.DateOfBirth)
            {
                patient.DateOfBirth = dto.DateOfBirth;
            }

            if (dto.Gender != patient.Gender)
            {
                patient.Gender = dto.Gender;
            }

            if (!string.IsNullOrWhiteSpace(dto.Address) &&
                dto.Address != patient.Address)
            {
                patient.Address = dto.Address;
            }

            if (dto.HivDiagnosisDate != patient.HivDiagnosisDate)
            {
                patient.HivDiagnosisDate = dto.HivDiagnosisDate;
            }

            if (!string.IsNullOrWhiteSpace(dto.ConsentInformation) &&
                dto.ConsentInformation != patient.ConsentInformation)
            {
                patient.ConsentInformation = dto.ConsentInformation;
            }

            if (!string.IsNullOrWhiteSpace(dto.AdditionalNotes) &&
                dto.AdditionalNotes != patient.AdditionalNotes)
            {
                patient.AdditionalNotes = dto.AdditionalNotes;
            }
            await _patientRepository.UpdateAsync(patient);
            return true;
        }

        public async Task<ApiResponse> CreatePatientAsync(CreatePatientRequest request)
        {
            try
            {
                var patientExists = await _patientRepository.GetByAccountIdAsync(request.AccountId);
                if (patientExists != null)
                {
                    return new ApiResponse("Error: Patient profile with this account already exists");
                }
                var patient = _mapper.Map<Patient>(request);
                await _patientRepository.AddAsync(patient);
                patient.PatientCodeAtFacility = await GenerateUniquePatientCodeAsync();
                return new ApiResponse("Patient created successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse($"Error creating patient: {ex.InnerException.Message}");
            }
        }

        private async Task<string> GenerateUniquePatientCodeAsync()
        {
            var random = new Random();
            string code;
            bool exists;

            do
            {
                code = "PT" + random.Next(1, 100000).ToString("D5");
                exists = await _patientRepository.AnyAsync(code);
            }
            while (exists);

            return code;
        }

        public async Task<ApiResponse> DeletePatientAsync(int id)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(id);
                if (patient == null)
                {
                    return new ApiResponse("Error: Patient not found");
                }
                await _patientRepository.DeleteAsync(patient);
                return new ApiResponse("Patient deleted successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse($"Error deleting patient: {ex.InnerException.Message}");
            }
        }

        public async Task<PatientResponse> GetPatientByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null) throw new ArgumentException($"Patient with ID {id} does not exist");
            var response = _mapper.Map<PatientResponse>(patient);
            return response;
        }

        public async Task<PatientResponse> GetPatientByAccountIdAsync(int accountId)
        {
            var patient = await _patientRepository.GetByAccountIdAsync(accountId);
            if (patient == null)
            {
                throw new ArgumentException($"Patient with Account ID {accountId} not found");
            }
            var response = _mapper.Map<PatientResponse>(patient);
            return response;
        }
    }
}

