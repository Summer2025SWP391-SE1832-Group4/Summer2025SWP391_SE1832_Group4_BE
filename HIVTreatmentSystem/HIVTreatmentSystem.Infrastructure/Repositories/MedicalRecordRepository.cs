using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Medical Record operations
    /// Mỗi Patient chỉ có một MedicalRecord duy nhất (1-to-1 relationship)
    /// </summary>
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly HIVDbContext _context;

        public MedicalRecordRepository(HIVDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MedicalRecord>> GetAllAsync()
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                    .ThenInclude(p => p.Account)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Account)
                .Include(m => m.TestResults)
                    .ThenInclude(tr => tr.Appointment)
                        .ThenInclude(a => a.Doctor)
                            .ThenInclude(d => d.Account)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<MedicalRecord?> GetByIdAsync(int id)
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                    .ThenInclude(p => p.Account)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Account)
                .Include(m => m.TestResults)
                    .ThenInclude(tr => tr.Appointment)
                        .ThenInclude(a => a.Doctor)
                            .ThenInclude(d => d.Account)
                .FirstOrDefaultAsync(m => m.MedicalRecordId == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId)
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                    .ThenInclude(p => p.Account)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Account)
                .Include(m => m.TestResults)
                    .ThenInclude(tr => tr.Appointment)
                        .ThenInclude(a => a.Doctor)
                            .ThenInclude(d => d.Account)
                .Where(m => m.PatientId == patientId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MedicalRecord>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                    .ThenInclude(p => p.Account)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Account)
                .Include(m => m.TestResults)
                    .ThenInclude(tr => tr.Appointment)
                        .ThenInclude(a => a.Doctor)
                            .ThenInclude(d => d.Account)
                .Where(m => m.DoctorId == doctorId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<MedicalRecord> CreateAsync(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();
            return medicalRecord;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Update(medicalRecord);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Remove(medicalRecord);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> HasMedicalRecordAsync(int patientId)
        {
            return await _context.MedicalRecords
                .AnyAsync(m => m.PatientId == patientId);
        }
    }
} 