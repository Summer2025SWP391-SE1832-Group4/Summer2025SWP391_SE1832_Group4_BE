using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Models.Pages;
using HIVTreatmentSystem.Application.Models.Requests;
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
        AppointmentTypeEnum? appointmentType,
        AppointmentStatus? status,
        AppointmentServiceEnum? appointmentService,
        DateOnly? startDate,
        DateOnly? endDate,
        bool isDescending,
        string? sortBy,
        int pageIndex,
        int pageSize);
        Task<AppointmentResponse?> GetAppointmentByIdAsync(int id);
        Task<ApiResponse> CreateAppointmentAsync(AppointmentRequest request);
        Task<ApiResponse> UpdateAppointmentAsync(int id, AppointmentUpdateRequest request);
        Task<ApiResponse> DeleteAppointmentAsync(int appointmentId);

        Task<ApiResponse> SetStatusScheduledAsync(int appointmentId);



    }
}
