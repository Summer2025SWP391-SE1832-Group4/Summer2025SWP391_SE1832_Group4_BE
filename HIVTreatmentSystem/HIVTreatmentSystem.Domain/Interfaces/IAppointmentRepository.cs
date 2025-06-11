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
        Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(string status);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(
            string? doctorName,
            string? patientName,
            string? appointmentType,
            AppointmentStatus? status,
            DateOnly? startDate,
            DateOnly? endDate,
            bool isDescending,
            string? sortBy);
        Task CreateAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);

        Task DeleteAsync(Appointment appointment);

        Task<List<Appointment>> GetAppointmentsByDoctorAsync(int doctorId, DateOnly date);



    }
} 