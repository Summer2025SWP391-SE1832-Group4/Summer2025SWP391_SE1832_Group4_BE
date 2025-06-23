using HIVTreatmentSystem.Application.Models.Doctor;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDetailDto>> GetAllDoctorsWithDetailsAsync();
        Task<DoctorDetailDto?> GetDoctorByIdWithDetailsAsync(int doctorId);
        Task<List<DoctorDetailDto>> GetDoctorsBySpecialtyAsync(DoctorSpecialtyEnum specialty);
        Task<List<DoctorResponse>> GetAvailableDoctorsAsync(DateOnly date, TimeOnly time, AppointmentTypeEnum specialty);
        Task<DoctorDetailDto?> GetDoctorByAccountIdWithDetailsAsync(int accountId);
    }
}