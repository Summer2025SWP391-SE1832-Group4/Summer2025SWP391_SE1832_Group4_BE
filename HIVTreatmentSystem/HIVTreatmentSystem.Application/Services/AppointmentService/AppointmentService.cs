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

namespace HIVTreatmentSystem.Application.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<PageResult<AppointmentResponse>> GetAllAppointmentsAsync(
        string? doctorName,
        string? patientName,
        string? appointmentType,
        AppointmentStatus? status,
        DateOnly? startDate,
        DateOnly? endDate,
        bool isDescending,
        string? sortBy,
        int pageIndex,
        int pageSize)
        {
            var appointments = await _appointmentRepository.GetAllAppointmentsAsync(
                doctorName,
                patientName,
                appointmentType,
                status,
                startDate,
                endDate,
                isDescending,
                sortBy);

            var paged = appointments
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtoList = _mapper.Map<List<AppointmentResponse>>(paged);

            return new PageResult<AppointmentResponse>(dtoList, pageSize, pageIndex, appointments.Count());
        }

        public async Task<AppointmentResponse?> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(id);

            if (appointment == null)
                return null;

            return _mapper.Map<AppointmentResponse>(appointment);
        }

        public async Task<ApiResponse> CreateAppointmentAsync(AppointmentRequest request)
        {
            try
            {
                var dayOfWeek = request.AppointmentDate.DayOfWeek;
                if (dayOfWeek == DayOfWeek.Sunday)
                {
                    return new ApiResponse("Error: Can't create appointment on Sunday.");
                }

                var time = request.AppointmentTime;

                bool isMorning = time >= new TimeOnly(8, 0) && time <= new TimeOnly(11, 30);
                bool isAfternoon = time >= new TimeOnly(13, 0) && time <= new TimeOnly(16, 30);

                if (!isMorning && !isAfternoon)
                {
                    return new ApiResponse("Error: Please create in range 8:00 - 11:30 & 13:00 - 16:30");
                }

                if (time.Minute != 0 && time.Minute != 30)
                {
                    return new ApiResponse("Error: Please choose 8:00, 8:30, 9:00...");
                }

                var existingAppointments = await _appointmentRepository
            .GetAppointmentsByDoctorAsync(request.DoctorId, request.AppointmentDate);

                if (existingAppointments.Any(a => a.AppointmentTime == request.AppointmentTime))
                {
                    return new ApiResponse("Error: The doctor is already scheduled at this time..");
                }

                var appointment = _mapper.Map<Appointment>(request);
                await _appointmentRepository.AddAsync(appointment);
                return new ApiResponse("Appointment created successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse("Error: Failed to create appointment: " + ex.InnerException.Message);
            }
        }

        public async Task<ApiResponse> UpdateAppointmentAsync(int id, AppointmentRequest request)
        {
            try
            {

                var appointment = await _appointmentRepository.GetByIdAsync(id);
                if (appointment == null)
                    return new ApiResponse("Error: Appointment not found");

                var dayOfWeek = request.AppointmentDate.DayOfWeek;
                if (dayOfWeek == DayOfWeek.Sunday)
                {
                    return new ApiResponse("Error: Can't create appointment on Sunday.");
                }

                var time = request.AppointmentTime;

                bool isMorning = time >= new TimeOnly(8, 0) && time <= new TimeOnly(11, 30);
                bool isAfternoon = time >= new TimeOnly(13, 0) && time <= new TimeOnly(16, 30);

                if (!isMorning && !isAfternoon)
                {
                    return new ApiResponse("Error: Please create in range 8:00 - 11:30 & 13:00 - 16:30");
                }

                if (time.Minute != 0 && time.Minute != 30)
                {
                    return new ApiResponse("Error: Please choose 8:00, 8:30, 9:00...");
                }

                var existingAppointments = await _appointmentRepository
            .GetAppointmentsByDoctorAsync(request.DoctorId, request.AppointmentDate);

                if (existingAppointments.Any(a => a.AppointmentTime == request.AppointmentTime))
                {
                    return new ApiResponse("Error: The doctor is already scheduled at this time..");
                }

                _mapper.Map(request, appointment);

                await _appointmentRepository.UpdateAsync(appointment);
                return new ApiResponse("Appointment updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse("Error: Failed to update appointment: " + ex.InnerException.Message);
            }
        }

        public async Task<ApiResponse> DeleteAppointmentAsync(int appointmentId)
        {
            try
            {
                var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
                if (appointment == null)
                    return new ApiResponse("Error: Appointment not found");
                await _appointmentRepository.DeleteAsync(appointment);
                return new ApiResponse("Appointment deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse("Error: Failed to update appointment: " + ex.InnerException.Message);
            }
        }



    }
}
