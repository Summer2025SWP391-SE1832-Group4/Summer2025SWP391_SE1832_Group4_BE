using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Doctor;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Application.Services.DoctorService
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<List<DoctorDetailDto>> GetAllDoctorsWithDetailsAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return doctors.Select(MapToDoctorDetails).ToList();
        }

        public async Task<DoctorDetailDto?> GetDoctorByIdWithDetailsAsync(int doctorId)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            return doctor != null ? MapToDoctorDetails(doctor) : null;
        }

        public async Task<List<DoctorDetailDto>> GetDoctorsBySpecialtyAsync(string specialty)
        {
            var doctors = await _doctorRepository.FindAsync(d => d.Specialty == specialty);
            return doctors.Select(MapToDoctorDetails).ToList();
        }

        private DoctorDetailDto MapToDoctorDetails(Doctor doctor)
        {
            return new DoctorDetailDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.Account.FullName,
                Email = doctor.Account.Email,
                PhoneNumber = doctor.Account.PhoneNumber,
                Specialty = doctor.Specialty,
                Qualifications = doctor.Qualifications,
                YearsOfExperience = doctor.YearsOfExperience,
                ShortDescription = doctor.ShortDescription,
                ExperienceWorkings = doctor.ExperienceWorkings.Select(e => new ExperienceWorkingDto
                {
                    ExperienceId = e.Id,
                    DoctorId = e.DoctorId,
                    HospitalName = e.HospitalName,
                    Position = e.Position,
                    StartDate = e.FromDate,
                    EndDate = e.ToDate
                }).ToList(),
                Certificates = doctor.Certificates.Select(c => new CertificateDto
                {
                    CertificateId = c.CertificateId,
                    DoctorId = c.DoctorId,
                    Title = c.Title,
                    Description = c.Description,
                    IssuedDate = c.IssuedDate,
                    IssuedBy = c.IssuedBy
                }).ToList()
            };
        }
    }
}
