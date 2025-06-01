using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
