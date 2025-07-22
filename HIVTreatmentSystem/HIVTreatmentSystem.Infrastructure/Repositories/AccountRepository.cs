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

        public async Task<bool> GetByPhoneNumberExitsAsync(string phoneNumber)
        {
            return await _context.Accounts.AnyAsync(phone => phone.PhoneNumber == phoneNumber);
        }

        public async Task<Account?> GetByUsernameAsync(string username)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Accounts.AnyAsync(a => a.Username == username);
        }

        public async Task AddAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<Account?> GetByPhoneAsync(string phone)
        {
            return await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.PhoneNumber == phone);
        }
        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Account> GetByResetTokenAsync(String token)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.PasswordResetToken == token);
        }
    }
}
