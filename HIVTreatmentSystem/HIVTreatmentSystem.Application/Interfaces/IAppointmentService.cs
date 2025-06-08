using HIVTreatmentSystem.Application.Models.Pages;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<PageResult<AppointmentResponse>> GetAllAppointmentsAsync(
        string? doctorName,
        string? patientName,
        string? appointmentType,
        AppointmentStatus? status,
        DateTime? startDate,
        DateTime? endDate,
        bool isDescending,
        string? sortBy,
        int pageIndex,
        int pageSize);
    }
}
