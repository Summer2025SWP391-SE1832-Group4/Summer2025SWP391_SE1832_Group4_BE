
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class BlogTagRepository : GenericRepository<BlogTag, int>, IBlogTagRepository
    {
        private readonly HIVDbContext _context;

        public BlogTagRepository(HIVDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<BlogTag> Items, int TotalCount)> GetPagedAsync(
            string? nameFilter,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        )
        {
            var query = _context.BlogTags.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                var nf = nameFilter.Trim().ToLower();
                query = query.Where(t => t.Name.ToLower().Contains(nf));
            }

            var total = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "name":
                        query = sortDesc
                            ? query.OrderByDescending(t => t.Name)
                            : query.OrderBy(t => t.Name);
                        break;
                    default:
                        query = sortDesc
                            ? query.OrderByDescending(t => t.BlogTagId)
                            : query.OrderBy(t => t.BlogTagId);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(t => t.BlogTagId);
            }

            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, total);
        }
    }
}
