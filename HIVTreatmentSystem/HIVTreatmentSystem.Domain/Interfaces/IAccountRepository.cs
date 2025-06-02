using HIVTreatmentSystem.Domain.Entities;


namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(int id);
        Task<Account?> UpdateAsync(Account account);
    }
}
