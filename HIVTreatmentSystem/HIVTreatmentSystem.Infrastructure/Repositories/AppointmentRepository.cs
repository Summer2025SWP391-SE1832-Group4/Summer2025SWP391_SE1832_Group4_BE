using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HIVDbContext _context;

        public AppointmentRepository(HIVDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment?> GetAppointmentWithDetailsAsync(int appointmentId)
        {
            return await _context
                .Appointments.Include(a => a.Patient)
                .ThenInclude(p => p.Account)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Account)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId)
        {
            return await _context.Appointments.Where(a => a.PatientId == patientId).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId)
        {
            return await _context.Appointments.Where(a => a.DoctorId == doctorId).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(string status)
        {
            return await _context
                .Appointments.Where(a => a.Status.ToString() == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(
            string? doctorName,
            string? patientName,
            AppointmentTypeEnum? appointmentType,
            AppointmentStatus? status,
            AppointmentServiceEnum? appointmentServiceEnum,
            DateOnly? startDate,
            DateOnly? endDate,
            int? accountId,
            bool isDescending,
            string? sortBy
        )
        {
            var query = _context
                .Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Account)
                .Include(a => a.Patient)
                .ThenInclude(p => p.Account)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(doctorName))
                query = query.Where(a => a.Doctor.Account.FullName.Contains(doctorName));

            if (!string.IsNullOrWhiteSpace(patientName))
                query = query.Where(a => a.Patient.Account.FullName.Contains(patientName));

            if (appointmentType.HasValue)
                query = query.Where(a => a.AppointmentType == appointmentType.Value);

            if (appointmentServiceEnum.HasValue)
                query = query.Where(a => a.AppointmentService == appointmentServiceEnum.Value);

            if (status.HasValue)
                query = query.Where(a => a.Status == status.Value);

            if (accountId.HasValue)
                query = query.Where(a => a.Doctor.AccountId == accountId.Value || a.Patient.AccountId == accountId.Value);


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
                    : query.OrderBy(a => a.AppointmentDate),
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

        public async Task<List<Appointment>> GetAppointmentsByDoctorAsync(
            int doctorId,
            DateOnly date
        )
        {
            return await _context
                .Appointments.Where(a => a.DoctorId == doctorId && a.AppointmentDate == date)
                .ToListAsync();
        }

        public async Task<bool> AnyAsync(Patient patient)
        {
            return await _context.Appointments.AnyAsync(a =>
                a.PatientId == patient.PatientId
                && (
                    a.Status == AppointmentStatus.Scheduled
                    || a.Status == AppointmentStatus.PendingConfirmation
                )
            );
        }

        public async Task<List<Appointment>> GetAppointmentsByAccountIdAsync(int accountId)
        {
            return await _context
                .Appointments.Include(a => a.Doctor)
                .ThenInclude(d => d.Account)
                .Include(a => a.Patient)
                .ThenInclude(p => p.Account)
                .Where(a => a.Patient.AccountId == accountId)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByDateAsync(
            DateOnly date,
            string? phoneNumber = null
        )
        {
            var query = _context
                .Appointments.Include(a => a.Patient)
                .ThenInclude(p => p.Account)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Account)
                .Where(a => a.AppointmentDate == date && a.Status == AppointmentStatus.Scheduled)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                query = query.Where(a =>
                    a.Patient.Account.PhoneNumber != null
                    && a.Patient.Account.PhoneNumber.Contains(phoneNumber)
                );
            }

            return await query.OrderBy(a => a.AppointmentTime).ToListAsync();
        }

        public async Task<List<int>> GetDoctorIdsByDateAndTimeAsync(DateOnly date, TimeOnly time)
        {
            return await _context.Appointments
                .Where(a => a.AppointmentDate == date && a.AppointmentTime == time)
                .Select(a => a.DoctorId)
                .Distinct()
                .ToListAsync();
        }
    }
}
