using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return await _context.Accounts.Include(a => a.Role).FirstOrDefaultAsync(a => a.Email == email);
        }
            IQueryable<Account> query = _context.Accounts.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(usernameFilter))
        public async Task<Account?> GetByUsernameAsync(string username)
        {
                var lower = usernameFilter.Trim().ToLower();
                query = query.Where(a => a.Username.ToLower().Contains(lower));
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
            if (!string.IsNullOrWhiteSpace(emailFilter))
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
                var lower = emailFilter.Trim().ToLower();
                query = query.Where(a => a.Email.ToLower().Contains(lower));
        }

        public async Task<bool> UsernameExistsAsync(string username)
            if (statusFilter.HasValue)
        {
            return await _context.Accounts.AnyAsync(a => a.Username == username);
                query = query.Where(a => a.AccountStatus == statusFilter.Value);
        }

        public async Task AddAsync(Account account)
            if (roleIdFilter.HasValue)
        {
            await _context.Accounts.AddAsync(account);
                query = query.Where(a => a.RoleId == roleIdFilter.Value);
        }

        public async Task SaveChangesAsync()
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

        public async Task<Role?> GetRoleByIdAsync(int roleId)
            else
        {
            return await _context.Roles.FindAsync(roleId);
                query = query.OrderByDescending(a => a.CreatedAt);
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
            int skip = (pageNumber - 1) * pageSize;
            var items = await query.Skip(skip).Take(pageSize).ToListAsync();

            return (Accounts: items, TotalCount: totalCount);
        }
    }
} 