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

            patient.DateOfBirth = dto.DateOfBirth;
            patient.Gender = dto.Gender;
            patient.Address = dto.Address;
            patient.HivDiagnosisDate = dto.HivDiagnosisDate;
            patient.ConsentInformation = dto.ConsentInformation;
            patient.AdditionalNotes = dto.AdditionalNotes;

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
            } catch (Exception ex)
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
    }
}
