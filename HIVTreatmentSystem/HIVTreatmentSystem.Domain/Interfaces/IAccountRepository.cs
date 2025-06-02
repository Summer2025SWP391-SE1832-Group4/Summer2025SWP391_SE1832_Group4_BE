using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account, int>
    {
        Task<Account?> GetByEmailAsync(string email);
        Task<Account?> GetByUsernameAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task AddAsync(Account account);
        Task SaveChangesAsync();
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<List<Role>> GetAllRolesAsync();
    }
} 