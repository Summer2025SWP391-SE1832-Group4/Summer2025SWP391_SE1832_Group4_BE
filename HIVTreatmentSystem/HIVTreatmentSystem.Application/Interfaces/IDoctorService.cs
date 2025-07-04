using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Models.Doctor;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Enums;


namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDetailDto>> GetAllDoctorsWithDetailsAsync();
        Task<DoctorDetailDto?> GetDoctorByIdWithDetailsAsync(int doctorId);
        Task<List<DoctorDetailDto>> GetDoctorsBySpecialtyAsync(DoctorSpecialtyEnum specialty);
        Task<List<DoctorResponse>> GetAvailableDoctorsAsync(DateOnly date, TimeOnly time, AppointmentTypeEnum specialty);
        Task<DoctorDetailDto?> GetDoctorByAccountIdWithDetailsAsync(int accountId);

        Task<ApiResponse> CreateDoctorAsync(CreateDoctorRequest request, DoctorSpecialtyEnum? doctorSpecialty);

        Task<ApiResponse> DeleteDoctorAsync(int doctorId);

        Task<bool> UpdateDoctorAsync(int doctorId, UpdateDoctorRequest request, DoctorSpecialtyEnum? specialty);
    }
}