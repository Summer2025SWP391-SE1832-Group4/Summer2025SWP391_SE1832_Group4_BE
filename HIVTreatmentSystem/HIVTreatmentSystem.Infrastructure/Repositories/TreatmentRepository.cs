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
                .Include(t => t.Regimen)
                .Include(t => t.PrescribingDoctor)
                .FirstOrDefaultAsync(t => t.PatientTreatmentId == patientTreatmentId);
        }

        public async Task<IEnumerable<PatientTreatment>> GetTreatmentsByPatientAsync(int patientId)
        {
            return await _context.PatientTreatments
                .Where(t => t.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PatientTreatment>> GetTreatmentsByDoctorAsync(int doctorId)
        {
            return await _context.PatientTreatments
                .Where(t => t.PrescribingDoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PatientTreatment>> GetActiveTreatmentsAsync()
        {
            return await _context.PatientTreatments
                .Where(t => t.Status == Domain.Enums.TreatmentStatus.InTreatment)
                .ToListAsync();
        }

        public async Task<IEnumerable<PatientTreatment>> GetTreatmentsByStatusAsync(string status)
        {
            return await _context.PatientTreatments
                .Where(t => t.Status.ToString() == status)
                .ToListAsync();
        }
    }
}
