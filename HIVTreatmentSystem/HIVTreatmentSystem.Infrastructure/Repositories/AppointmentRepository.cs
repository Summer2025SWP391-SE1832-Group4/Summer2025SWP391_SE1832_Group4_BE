using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
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

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Appointments
                .Where(a => a.AppointmentDateTime >= now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Appointments
                .Where(a => a.AppointmentDateTime >= startDate && a.AppointmentDateTime <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(string status)
        {
            return await _context.Appointments
                .Where(a => a.Status.ToString() == status)
                .ToListAsync();
        }
    }
}
