using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class TreatmentRepository : GenericRepository<PatientTreatment, int>, ITreatmentRepository
    {
        public TreatmentRepository(HIVDbContext context) : base(context)
        {
        }

        public async Task<PatientTreatment?> GetTreatmentWithDetailsAsync(int patientTreatmentId)
        {
            return await _context.PatientTreatments
                .Include(t => t.Patient)
                    .ThenInclude(p => p.Account)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.TestResults)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.MedicalRecords)
                .Include(t => t.Regimen)
                .Include(t => t.PrescribingDoctor)
                    .ThenInclude(d => d.Account)
                .FirstOrDefaultAsync(t => t.PatientTreatmentId == patientTreatmentId);
        }

        public async Task<IEnumerable<PatientTreatment>> GetTreatmentsByPatientAsync(int patientId)
        {
            return await _context.PatientTreatments
                .Include(t => t.Patient)
                    .ThenInclude(p => p.Account)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.TestResults)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.MedicalRecords)
                .Include(t => t.Regimen)
                .Include(t => t.PrescribingDoctor)
                    .ThenInclude(d => d.Account)
                .Where(t => t.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PatientTreatment>> GetTreatmentsByDoctorAsync(int doctorId)
        {
            return await _context.PatientTreatments
                .Include(t => t.Patient)
                    .ThenInclude(p => p.Account)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.TestResults)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.MedicalRecords)
                .Include(t => t.Regimen)
                .Include(t => t.PrescribingDoctor)
                    .ThenInclude(d => d.Account)
                .Where(t => t.PrescribingDoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PatientTreatment>> GetActiveTreatmentsAsync()
        {
            return await _context.PatientTreatments
                .Include(t => t.Patient)
                    .ThenInclude(p => p.Account)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.TestResults)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.MedicalRecords)
                .Include(t => t.Regimen)
                .Include(t => t.PrescribingDoctor)
                    .ThenInclude(d => d.Account)
                .Where(t => t.Status == Domain.Enums.TreatmentStatus.InTreatment)
                .ToListAsync();
        }

        public async Task<IEnumerable<PatientTreatment>> GetTreatmentsByStatusAsync(string status)
        {
            return await _context.PatientTreatments
                .Include(t => t.Patient)
                    .ThenInclude(p => p.Account)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.TestResults)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.MedicalRecords)
                .Include(t => t.Regimen)
                .Include(t => t.PrescribingDoctor)
                    .ThenInclude(d => d.Account)
                .Where(t => t.Status.ToString() == status)
                .ToListAsync();
        }

        // Override GetAllAsync to include related data
        public async Task<IEnumerable<PatientTreatment>> GetAllAsync()
        {
            return await _context.PatientTreatments
                .Include(t => t.Patient)
                    .ThenInclude(p => p.Account)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.TestResults)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.MedicalRecords)
                .Include(t => t.Regimen)
                .Include(t => t.PrescribingDoctor)
                    .ThenInclude(d => d.Account)
                .ToListAsync();
        }

        // Override GetByIdAsync to include related data
        public  async Task<PatientTreatment?> GetByIdAsync(int id)
        {
            return await _context.PatientTreatments
                .Include(t => t.Patient)
                    .ThenInclude(p => p.Account)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.TestResults)
                .Include(t => t.Patient)
                    .ThenInclude(p => p.MedicalRecords)
                .Include(t => t.Regimen)
                .Include(t => t.PrescribingDoctor)
                    .ThenInclude(d => d.Account)
                .FirstOrDefaultAsync(t => t.PatientTreatmentId == id);
        }

        public async Task<PatientTreatment> AddAsync(PatientTreatment treatment)
        {
            _context.PatientTreatments.Add(treatment);
            await _context.SaveChangesAsync();
            return treatment;
        }

        public async Task<PatientTreatment> UpdateAsync(PatientTreatment treatment)
        {
            _context.Entry(treatment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return treatment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var treatment = await _context.PatientTreatments.FindAsync(id);
            if (treatment == null) return false;
            _context.PatientTreatments.Remove(treatment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
