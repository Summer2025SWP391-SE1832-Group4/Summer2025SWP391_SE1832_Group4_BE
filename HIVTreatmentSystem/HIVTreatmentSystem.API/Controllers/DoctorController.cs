using System.Threading.Tasks;
using HIVTreatmentSystem.API.Controllers;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    [ApiController]
    [Route("api/doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        /// <summary>
        /// Get all doctors with their details including account, experience, certificates, and schedules
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsWithDetailsAsync();
            return Ok(new ApiResponse("Success", doctors));
        }

        /// <summary>
        /// Get a specific doctor by ID with all their details
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdWithDetailsAsync(id);
            if (doctor == null)
                return NotFound(new ApiResponse("Doctor not found"));
            return Ok(new ApiResponse("Success", doctor));
        }

        /// <summary>
        /// Get doctors by specialty
        /// </summary>
        [HttpGet("specialty/{specialty}")]
        public async Task<IActionResult> GetDoctorsBySpecialty(string specialty)
        {
            var doctors = await _doctorService.GetDoctorsBySpecialtyAsync(specialty);
            return Ok(new ApiResponse("Success", doctors));
        }
    }
} 