using HIVTreatmentSystem.Application.Repositories;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for managing test results
    /// </summary>
    public class TestResultRepository : ITestResultRepository
    {
        private readonly HIVDbContext _context;

        /// <summary>
        /// Constructor for TestResultRepository
        /// </summary>
        /// <param name="context">Database context</param>
        public TestResultRepository(HIVDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TestResult>> GetAllAsync()
        {
            return await _context.TestResults
                .Include(t => t.Patient)
                .Include(t => t.MedicalRecord)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<TestResult?> GetByIdAsync(int id)
        {
            return await _context.TestResults
                .Include(t => t.Patient)
                .Include(t => t.MedicalRecord)
                .FirstOrDefaultAsync(t => t.TestResultId == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TestResult>> GetByPatientIdAsync(int patientId)
        {
            return await _context.TestResults
                .Include(t => t.Patient)
                .Include(t => t.MedicalRecord)
                .Where(t => t.PatientId == patientId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TestResult>> GetByMedicalRecordIdAsync(int medicalRecordId)
        {
            return await _context.TestResults
                .Include(t => t.Patient)
                .Include(t => t.MedicalRecord)
                .Where(t => t.MedicalRecordId == medicalRecordId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TestResult>> GetByAppointmentIdAsync(int appointmentId)
        {
            return await (from t in _context.TestResults
                    .Include(t => t.Patient)
                    .Include(t => t.MedicalRecord)
                join a in _context.Appointments on t.PatientId equals a.PatientId
                where a.AppointmentId == appointmentId &&
                      (a.AppointmentService == AppointmentServiceEnum.ELISA ||
                       a.AppointmentService == AppointmentServiceEnum.PCR ||
                       a.AppointmentService == AppointmentServiceEnum.RapidTest ||
                       a.AppointmentService == AppointmentServiceEnum.PostTestCounseling ||
                       a.AppointmentService == AppointmentServiceEnum.PreTestCounseling) && a.AppointmentType == AppointmentTypeEnum.Testing
                select t).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<TestResult> AddAsync(TestResult testResult)
        {
            _context.TestResults.Add(testResult);
            await _context.SaveChangesAsync();
            return testResult;
        }

        /// <inheritdoc/>
        public async Task<TestResult> UpdateAsync(TestResult testResult)
        {
            _context.Entry(testResult).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return testResult;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            var testResult = await _context.TestResults.FindAsync(id);
            if (testResult == null)
                return false;

            _context.TestResults.Remove(testResult);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 