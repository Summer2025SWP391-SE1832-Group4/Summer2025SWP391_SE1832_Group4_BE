using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account, int>, IAccountRepository
    {
        private readonly HIVDbContext _context;

        public AccountRepository(HIVDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Account> Accounts, int TotalCount)> GetPagedAsync(
            string? usernameFilter,
            string? emailFilter,
            AccountStatus? statusFilter,
            int? roleIdFilter,
            string? sortBy,
            bool sortDescending,
            int pageNumber,
            int pageSize
        )
        {
            IQueryable<Account> query = _context.Accounts.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(usernameFilter))
            {
                var lower = usernameFilter.Trim().ToLower();
                query = query.Where(a => a.Username.ToLower().Contains(lower));
            }

            if (!string.IsNullOrWhiteSpace(emailFilter))
            {
                var lower = emailFilter.Trim().ToLower();
                query = query.Where(a => a.Email.ToLower().Contains(lower));
            }

            if (statusFilter.HasValue)
            {
                query = query.Where(a => a.AccountStatus == statusFilter.Value);
            }

            if (roleIdFilter.HasValue)
            {
                query = query.Where(a => a.RoleId == roleIdFilter.Value);
            }

            int totalCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "username":
                        query = sortDescending
                            ? query.OrderByDescending(a => a.Username)
                            : query.OrderBy(a => a.Username);
                        break;
                    case "email":
                        query = sortDescending
                            ? query.OrderByDescending(a => a.Email)
                            : query.OrderBy(a => a.Email);
                        break;
                    case "createdat":
                        query = sortDescending
                            ? query.OrderByDescending(a => a.CreatedAt)
                            : query.OrderBy(a => a.CreatedAt);
                        break;
                    default:
                        query = sortDescending
                            ? query.OrderByDescending(a => a.CreatedAt)
                            : query.OrderBy(a => a.CreatedAt);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(a => a.CreatedAt);
            }

            int skip = (pageNumber - 1) * pageSize;
            var items = await query.Skip(skip).Take(pageSize).ToListAsync();

            return (Accounts: items, TotalCount: totalCount);
        }
    }
}
