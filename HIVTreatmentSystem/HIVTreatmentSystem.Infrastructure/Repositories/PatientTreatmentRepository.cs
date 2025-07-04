
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class PatientTreatmentRepository
        : GenericRepository<PatientTreatment, int>,
            IPatientTreatmentRepository
    {
        private readonly HIVDbContext _context;

        public PatientTreatmentRepository(HIVDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PatientTreatment>> GetByPatientIdAsync(int patientId)
        {
            return await _context
                .PatientTreatments.Include(pt => pt.Patient)
                .Include(pt => pt.Regimen)
                .Include(pt => pt.PrescribingDoctor)
                .ThenInclude(d => d.Account)
                .Where(pt => pt.PatientId == patientId)
                .ToListAsync();
        }
    }
}
