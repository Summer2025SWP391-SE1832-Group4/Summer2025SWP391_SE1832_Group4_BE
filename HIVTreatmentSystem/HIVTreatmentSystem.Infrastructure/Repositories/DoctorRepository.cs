using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor, int>, IDoctorRepository
    {
        private readonly HIVDbContext _context;

        public DoctorRepository(HIVDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> FindAsync(Expression<Func<Doctor, bool>> predicate)
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .Include(d => d.ExperienceWorkings)
                .Include(d => d.Certificates)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .FirstOrDefaultAsync(d => d.DoctorId == id);
        }

        public async Task<Doctor?> GetByAccountIdAsync(int accountId)
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .FirstOrDefaultAsync(d => d.AccountId == accountId);
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .Include(d => d.ExperienceWorkings)
                .Include(d => d.Certificates)
                .ToListAsync();
        }

        public async Task<Doctor?> GetDoctorWithDetailsAsync(int doctorId)
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .Include(d => d.ExperienceWorkings)
                .Include(d => d.Certificates)
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId);
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsBySpecialtyAsync(DoctorSpecialtyEnum specialty)
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .Include(d => d.ExperienceWorkings)
                .Include(d => d.Certificates)
                .Where(d => d.Specialty == specialty.ToString())
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsWithSchedulesAsync()
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .Include(d => d.ExperienceWorkings)
                .Include(d => d.Certificates)
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(string department)
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .ToListAsync();
        }

        public async Task<List<Doctor>> GetDoctorsNotInIdsAsync(List<int> excludedDoctorIds)
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .Where(d => !excludedDoctorIds.Contains(d.DoctorId))
                .ToListAsync();
        }

        public async Task<List<Doctor>> GetAvailableDoctorsAsync(List<int> excludedDoctorIds, DoctorSpecialtyEnum specialty)
        {
            return await _context.Doctors
                .Include(d => d.Account)
                .Where(d => !excludedDoctorIds.Contains(d.DoctorId) && d.Specialty == specialty.ToString())
                .ToListAsync();
        }

        public async Task DeleteAsync(Doctor doctor)
        {
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

        }
    }
}
