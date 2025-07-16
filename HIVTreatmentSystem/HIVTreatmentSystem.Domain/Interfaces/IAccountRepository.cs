
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account, int>
    {
        Task<(IEnumerable<Account> Accounts, int TotalCount)> GetPagedAsync(
            string? usernameFilter,
            string? emailFilter,
            AccountStatus? statusFilter,
            int? roleIdFilter,
            string? sortBy,
            bool sortDescending,
            int pageNumber,
            int pageSize
        );
        Task<Account?> GetByEmailAsync(string email);
        Task<Account?> GetByUsernameAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task AddAsync(Account account);
        Task SaveChangesAsync();
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<List<Role>> GetAllRolesAsync();
        Task<Account> GetByResetTokenAsync(String token);
    }
}
