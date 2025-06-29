using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Medical Record operations
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
                .Include(m => m.TestResult)
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .Include(m => m.AdditionalTestResults)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<MedicalRecord?> GetByIdAsync(int id)
        {
            return await _context.MedicalRecords
                .Include(m => m.TestResult)
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .Include(m => m.AdditionalTestResults)
                .FirstOrDefaultAsync(m => m.MedicalRecordId == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId)
        {
            return await _context.MedicalRecords
                .Include(m => m.TestResult)
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .Include(m => m.AdditionalTestResults)
                .Where(m => m.PatientId == patientId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MedicalRecord>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.MedicalRecords
                .Include(m => m.TestResult)
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .Include(m => m.AdditionalTestResults)
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

        /// <inheritdoc/>
        public async Task<MedicalRecord?> GetByPatientIdUniqueAsync(int patientId)
        {
            return await _context.MedicalRecords
                .Include(m => m.TestResult)
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .Include(m => m.AdditionalTestResults)
                .FirstOrDefaultAsync(m => m.PatientId == patientId);
        }
    }
} 