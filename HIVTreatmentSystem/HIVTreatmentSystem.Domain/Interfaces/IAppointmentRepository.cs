using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetAppointmentWithDetailsAsync(int appointmentId);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(string status);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(
            string? doctorName,
            string? patientName,
            AppointmentTypeEnum? appointmentType,
            AppointmentStatus? status,
            AppointmentServiceEnum? appointmentService,
            DateOnly? startDate,
            DateOnly? endDate,
            bool isDescending,
            string? sortBy
        );
        Task CreateAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);

        Task DeleteAsync(Appointment appointment);

        Task<List<Appointment>> GetAppointmentsByDoctorAsync(int doctorId, DateOnly date);

        Task<bool> AnyAsync(Patient patient);

        Task<List<Appointment>> GetAppointmentsByAccountIdAsync(int accountId);

        Task<List<Appointment>> GetAppointmentsByDateAsync(
            DateOnly date,
            string? phoneNumber = null
        );

        Task<List<int>> GetDoctorIdsByDateAndTimeAsync(DateOnly date, TimeOnly time);

    }
}
