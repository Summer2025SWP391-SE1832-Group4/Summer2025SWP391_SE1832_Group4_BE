using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Models.Doctor;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDetailDto>> GetAllDoctorsWithDetailsAsync();
        Task<DoctorDetailDto?> GetDoctorByIdWithDetailsAsync(int doctorId);
        Task<List<DoctorDetailDto>> GetDoctorsBySpecialtyAsync(string specialty);
    }
}