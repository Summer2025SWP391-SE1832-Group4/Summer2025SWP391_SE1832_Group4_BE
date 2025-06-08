using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment, int>
    {
        Task<Appointment?> GetAppointmentWithDetailsAsync(int appointmentId);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(string status);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(
    string? doctorName,
    string? patientName,
    string? appointmentType,
    AppointmentStatus? status,
    DateTime? startDate,
    DateTime? endDate,
    bool isDescending,
    string? sortBy);
    }
} 