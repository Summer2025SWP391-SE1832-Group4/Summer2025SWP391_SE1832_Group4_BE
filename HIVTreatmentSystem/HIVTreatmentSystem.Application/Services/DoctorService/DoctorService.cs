using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Doctor;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Services.DoctorService
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository, IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
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

        public async Task<List<DoctorDetailDto>> GetDoctorsBySpecialtyAsync(DoctorSpecialtyEnum specialty)
        {
            var doctors = await _doctorRepository.FindAsync(d => d.Specialty == specialty.ToString());
            return doctors.Select(MapToDoctorDetails).ToList();
        }

        public async Task<DoctorDetailDto?> GetDoctorByAccountIdWithDetailsAsync(int accountId)
        {
            var doctors = await _doctorRepository.FindAsync(d => d.AccountId == accountId);
            var doctor = doctors.FirstOrDefault();
            return doctor != null ? MapToDoctorDetails(doctor) : null;
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

        public async Task<List<DoctorResponse>> GetAvailableDoctorsAsync(DateOnly date, TimeOnly time, AppointmentTypeEnum specialty)
        {
            List<Doctor> availableDoctors;
            var busyDoctorIds = await _appointmentRepository.GetDoctorIdsByDateAndTimeAsync(date, time);
            if (specialty == AppointmentTypeEnum.Consultation)
            {
                availableDoctors = await _doctorRepository.GetAvailableDoctorsAsync(busyDoctorIds, DoctorSpecialtyEnum.Consultant);
            }

            else if (specialty == AppointmentTypeEnum.Testing)
            {
                availableDoctors = await _doctorRepository.GetAvailableDoctorsAsync(busyDoctorIds, DoctorSpecialtyEnum.Testing);
            }
            else if (specialty == AppointmentTypeEnum.Therapy)
            {
                availableDoctors = await _doctorRepository.GetAvailableDoctorsAsync(busyDoctorIds, DoctorSpecialtyEnum.Therapy);
            }
            else
            {
                throw new ArgumentException("Invalid appointment type specified.");
            }
            return _mapper.Map<List<DoctorResponse>>(availableDoctors);
        }
    }
}
