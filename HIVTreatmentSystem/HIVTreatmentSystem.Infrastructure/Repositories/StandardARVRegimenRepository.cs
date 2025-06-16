using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class StandardARVRegimenRepository
        : GenericRepository<StandardARVRegimen, int>,
            IStandardARVRegimenRepository
    {
        private readonly HIVDbContext _context;

        public StandardARVRegimenRepository(HIVDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<StandardARVRegimen> Items, int TotalCount)> GetPagedAsync(
            string? regimenNameFilter,
            string? targetPopulationFilter,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        )
        {
            var query = _context.Set<StandardARVRegimen>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(regimenNameFilter))
            {
                var rf = regimenNameFilter.Trim().ToLower();
                query = query.Where(r => r.RegimenName.ToLower().Contains(rf));
            }

            if (!string.IsNullOrWhiteSpace(targetPopulationFilter))
            {
                var tf = targetPopulationFilter.Trim().ToLower();
                query = query.Where(r =>
                    r.TargetPopulation != null && r.TargetPopulation.ToLower().Contains(tf)
                );
            }

            var total = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "regimenname":
                        query = sortDesc
                            ? query.OrderByDescending(r => r.RegimenName)
                            : query.OrderBy(r => r.RegimenName);
                        break;
                    case "targetpopulation":
                        query = sortDesc
                            ? query.OrderByDescending(r => r.TargetPopulation)
                            : query.OrderBy(r => r.TargetPopulation);
                        break;
                    default:
                        query = sortDesc
                            ? query.OrderByDescending(r => r.RegimenId)
                            : query.OrderBy(r => r.RegimenId);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(r => r.RegimenId);
            }

            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, total);
        }
    }
}
