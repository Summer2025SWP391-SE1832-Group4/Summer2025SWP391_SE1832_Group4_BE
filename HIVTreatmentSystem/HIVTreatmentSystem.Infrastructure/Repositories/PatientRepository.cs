using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class PatientRepository : GenericRepository<Patient, int>, IPatientRepository
    {
        public PatientRepository(HIVDbContext context) : base(context)
        {
        }

        public async Task<Patient?> GetPatientWithDetailsAsync(int patientId)
        {
            return await _context.Patients
                .Include(p => p.Treatments)
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.PatientId == patientId);
        }

        public async Task<IEnumerable<Patient>> GetPatientsByDoctorAsync(int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Select(a => a.Patient)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientsByTreatmentTypeAsync(string treatmentType)
        {
            return await _context.PatientTreatments
                .Where(t => t.RegimenAdjustments != null && t.RegimenAdjustments.Contains(treatmentType))
                .Select(t => t.Patient)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientsByHIVStatusAsync(string hivStatus)
        {
            return null;
        }

        public async Task<Patient?> GetByAccountIdAsync(int accountId)
        {
            return await _context.Patients
                .Include(p => p.Account)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);
        }

        public async Task<bool> AnyAsync(string patientCode)
        {
            return await _context.Patients.AnyAsync(p => p.PatientCodeAtFacility == patientCode);
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients
                .Include(p => p.Account)
                .FirstOrDefaultAsync(p => p.PatientId == id);
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync(
            int? accountId,
            DateTime? dateOfBirth,
            Gender? gender,
            string? address,
            DateTime? hivDiagnosisDate,
            string? consentInformation,
            bool isDescending = false,
            string? sortBy = "")
        {
            var query = _context.Patients
                .Include(p => p.Account)
                .AsQueryable();

            if (accountId.HasValue)
                query = query.Where(p => p.AccountId == accountId.Value);

            if (dateOfBirth.HasValue)
                query = query.Where(p => p.DateOfBirth == dateOfBirth.Value);

            if (gender.HasValue)
                query = query.Where(p => p.Gender == gender.Value);

            if (!string.IsNullOrWhiteSpace(address))
                query = query.Where(p => p.Address.Contains(address));

            if (hivDiagnosisDate.HasValue)
                query = query.Where(p => p.HivDiagnosisDate == hivDiagnosisDate.Value);

            if (!string.IsNullOrWhiteSpace(consentInformation))
                query = query.Where(p => p.ConsentInformation.Contains(consentInformation));

            query = sortBy?.ToLower() switch
            {
                "accountid" => isDescending ? query.OrderByDescending(p => p.AccountId) : query.OrderBy(p => p.AccountId),
                "dateofbirth" => isDescending ? query.OrderByDescending(p => p.DateOfBirth) : query.OrderBy(p => p.DateOfBirth),
                "gender" => isDescending ? query.OrderByDescending(p => p.Gender) : query.OrderBy(p => p.Gender),
                "address" => isDescending ? query.OrderByDescending(p => p.Address) : query.OrderBy(p => p.Address),
                "hvdiagnosisdate" => isDescending ? query.OrderByDescending(p => p.HivDiagnosisDate) : query.OrderBy(p => p.HivDiagnosisDate),
                "consentinformation" => isDescending ? query.OrderByDescending(p => p.ConsentInformation) : query.OrderBy(p => p.ConsentInformation),

                _ => isDescending
                    ? query.OrderByDescending(a => a.AccountId)
                    : query.OrderBy(a => a.AccountId),
            };

            return await query.ToListAsync();

        }

        public async Task DeleteAsync(Patient patient)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
    }
}
