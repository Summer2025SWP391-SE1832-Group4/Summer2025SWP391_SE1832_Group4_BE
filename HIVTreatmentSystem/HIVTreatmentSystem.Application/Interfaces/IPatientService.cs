using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Models.Pages;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Enums;


namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IPatientService
    {
        Task<PageResult<PatientResponse>> GetAllPatientsAsync(
            int? accountId,
            DateTime? dateOfBirth,
            Gender? gender,
            string? address,
            DateTime? hivDiagnosisDate,
            string? consentInformation,
            bool isDescending = false,
            string? sortBy = "",
            int pageIndex = 1,
            int pageSize = 10);
        Task<bool> UpdatePatientAsync(int patientId, UpdatePatientRequest dto);

        Task<ApiResponse> CreatePatientAsync(CreatePatientRequest request);

        Task<ApiResponse> DeletePatientAsync(int id);

        Task<PatientResponse> GetPatientByIdAsync(int id);

        Task<PatientResponse> GetPatientByAccountIdAsync(int accountId);
    }
}
