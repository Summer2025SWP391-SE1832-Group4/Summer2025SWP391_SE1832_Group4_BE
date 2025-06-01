using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

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
    }
} 