using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
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

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<bool> UpdatePatientAsync(int patientId, UpdatePatientRequest dto)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null) return false;

            // Map fields
            patient.DateOfBirth = dto.DateOfBirth;
            patient.Gender = dto.Gender;
            patient.Address = dto.Address;
            patient.HivDiagnosisDate = dto.HivDiagnosisDate;
            patient.ConsentInformation = dto.ConsentInformation;
            patient.AdditionalNotes = dto.AdditionalNotes;

            await _patientRepository.UpdateAsync(patient);
            return true;
        }
    }
}
