using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
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

        public async Task<(IEnumerable<PatientTreatment> Items, int TotalCount)> GetPagedAsync(
            string? statusFilter,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        )
        {
            // Khởi tạo query dưới dạng IQueryable để gán Include/Where/OrderBy dễ dàng
            IQueryable<PatientTreatment> query = _context
                .PatientTreatments.AsNoTracking()
                .Include(pt => pt.Patient)
                .Include(pt => pt.Regimen)
                .Include(pt => pt.PrescribingDoctor)
                .ThenInclude(doc => doc.Account);

            // Filter theo status nếu có
            if (
                !string.IsNullOrWhiteSpace(statusFilter)
                && Enum.TryParse<TreatmentStatus>(statusFilter.Trim(), ignoreCase: true, out var st)
            )
            {
                query = query.Where(pt => pt.Status == st);
            }

            // Đếm tổng sau filter
            var total = await query.CountAsync();

            // Sort
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.Trim().ToLowerInvariant())
                {
                    case "status":
                        query = sortDesc
                            ? query.OrderByDescending(pt => pt.Status)
                            : query.OrderBy(pt => pt.Status);
                        break;
                    case "patientid":
                        query = sortDesc
                            ? query.OrderByDescending(pt => pt.PatientId)
                            : query.OrderBy(pt => pt.PatientId);
                        break;
                    default:
                        query = sortDesc
                            ? query.OrderByDescending(pt => pt.PatientTreatmentId)
                            : query.OrderBy(pt => pt.PatientTreatmentId);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(pt => pt.PatientTreatmentId);
            }

            // Paging
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, total);
        }
    }
}
