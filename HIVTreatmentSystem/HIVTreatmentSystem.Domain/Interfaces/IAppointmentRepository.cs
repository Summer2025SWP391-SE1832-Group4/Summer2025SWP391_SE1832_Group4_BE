using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<Appointment> GetAppointmentWithDetailsAsync(Guid id);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(Guid patientId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(Guid doctorId);
        Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(string status);
    }
} 