using AutoMapper;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Pages;
using HIVTreatmentSystem.Application.Models.Responses;
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
        DateTime? startDate,
        DateTime? endDate,
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
    }
}
