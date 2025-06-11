using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment, int>, IAppointmentRepository
    {
        public AppointmentRepository(HIVDbContext context) : base(context)
        {
        }

        public async Task<Appointment?> GetAppointmentWithDetailsAsync(int appointmentId)
        {
            return await _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p.Account)
                .Include(a => a.Doctor).ThenInclude(d => d.Account)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

       

        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(string status)
        {
            return await _context.Appointments
                .Where(a => a.Status.ToString() == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(
    string? doctorName,
    string? patientName,
    string? appointmentType,
    AppointmentStatus? status,
    DateOnly? startDate,
    DateOnly? endDate,
    bool isDescending,
    string? sortBy)
        {
            var query = _context.Appointments
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Account)
                .Include(a => a.Patient)
                    .ThenInclude(p => p.Account)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(doctorName))
                query = query.Where(a => a.Doctor != null && a.Doctor.Account.FullName.Contains(doctorName));

            if (!string.IsNullOrWhiteSpace(patientName))
                query = query.Where(a => a.Patient.Account.FullName.Contains(patientName));

            if (!string.IsNullOrWhiteSpace(appointmentType))
                query = query.Where(a => a.AppointmentType != null && a.AppointmentType.Contains(appointmentType));

            if (status.HasValue)
                query = query.Where(a => a.Status == status.Value);

            if (startDate.HasValue)
            {
                query = query.Where(a => a.AppointmentDate >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.AppointmentDate <= endDate);
            }

            query = sortBy?.ToLower() switch
            {
                "doctorname" => isDescending
                    ? query.OrderByDescending(a => a.Doctor.Account.FullName)
                    : query.OrderBy(a => a.Doctor.Account.FullName),

                "patientname" => isDescending
                    ? query.OrderByDescending(a => a.Patient.Account.FullName)
                    : query.OrderBy(a => a.Patient.Account.FullName),

                "appointmentdate" => isDescending
                    ? query.OrderByDescending(a => a.AppointmentDate)
                    : query.OrderBy(a => a.AppointmentDate),

                "status" => isDescending
                    ? query.OrderByDescending(a => a.Status)
                    : query.OrderBy(a => a.Status),

                "type" => isDescending
                    ? query.OrderByDescending(a => a.AppointmentType)
                    : query.OrderBy(a => a.AppointmentType),

                _ => isDescending
                    ? query.OrderByDescending(a => a.AppointmentDate)
                    : query.OrderBy(a => a.AppointmentDate)
            };

            return await query.ToListAsync();
        }

        public async Task CreateAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByDoctorAsync(int doctorId, DateOnly date)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == date)
                .ToListAsync();
        }

    }
}
