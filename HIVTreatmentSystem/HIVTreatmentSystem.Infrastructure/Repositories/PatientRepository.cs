using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
    }
}
