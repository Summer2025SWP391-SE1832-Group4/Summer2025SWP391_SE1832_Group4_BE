
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class BlogRepository : GenericRepository<Blog, int>, IBlogRepository
    {
        private readonly HIVDbContext _context;

        public BlogRepository(HIVDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Blog> Items, int TotalCount)> GetPagedAsync(
            string? titleFilter,
            string? contentFilter,
            int? tagIdFilter,
            DateTime? createdFrom,
            DateTime? createdTo,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        )
        {
            var query = _context.Blogs.Include(b => b.BlogTag).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(titleFilter))
                query = query.Where(b => b.Title.ToLower().Contains(titleFilter.Trim().ToLower()));

            if (!string.IsNullOrWhiteSpace(contentFilter))
                query = query.Where(b =>
                    b.Content.ToLower().Contains(contentFilter.Trim().ToLower())
                );

            if (tagIdFilter.HasValue)
                query = query.Where(b => b.BlogTagId == tagIdFilter.Value);

            if (createdFrom.HasValue)
                query = query.Where(b => b.CreatedAt >= createdFrom.Value);

            if (createdTo.HasValue)
                query = query.Where(b => b.CreatedAt <= createdTo.Value);

            var total = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "title":
                        query = sortDesc
                            ? query.OrderByDescending(b => b.Title)
                            : query.OrderBy(b => b.Title);
                        break;
                    case "createdat":
                        query = sortDesc
                            ? query.OrderByDescending(b => b.CreatedAt)
                            : query.OrderBy(b => b.CreatedAt);
                        break;
                    default:
                        query = sortDesc
                            ? query.OrderByDescending(b => b.BlogId)
                            : query.OrderBy(b => b.BlogId);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(b => b.BlogId);
            }

            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, total);
        }
    }
}
