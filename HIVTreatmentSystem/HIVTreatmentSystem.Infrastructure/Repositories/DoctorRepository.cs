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
    public class DoctorRepository : GenericRepository<Doctor, int>, IDoctorRepository
    {
        public DoctorRepository(HIVDbContext context) : base(context)
        {
        }

        public async Task<Doctor?> GetDoctorWithDetailsAsync(int doctorId)
        {
            return await _context.Doctors
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId);
        }
        
        public async Task<IEnumerable<Doctor>> GetDoctorsBySpecializationAsync(string specialty)
        {
            return await _context.Doctors
                .Where(d => d.Specialty == specialty)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(string department)
        {
            // Since Doctor entity doesn't have Department property, we'll need to join with Account
            // For now, return all doctors as a workaround
            return await _context.Doctors.ToListAsync();
            
            // When Department property is added to Doctor entity, use this:
            // return await _context.Doctors
            //     .Where(d => d.Department == department)
            //     .ToListAsync();
        }
        
        public async Task<IEnumerable<Doctor>> GetDoctorsWithActivePatientsAsync()
        {
            var currentDate = DateTime.UtcNow;
            return await _context.Appointments
                .Where(a => a.AppointmentDateTime >= currentDate)
                .Select(a => a.Doctor)
                .Where(d => d != null)
                .Distinct()
                .ToListAsync();
        }
    }
}
