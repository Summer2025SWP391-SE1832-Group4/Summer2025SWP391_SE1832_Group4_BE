using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IAccountService
    {
        Task<ListAccountsResponse> GetPagedAsync(
            ListAccountsRequest request,
            CancellationToken cancellationToken = default
        );

        Task<AccountResponse> GetByIdAsync(
            int accountId,
            CancellationToken cancellationToken = default
        );

        Task<AccountResponse> CreateAsync(
            AccountRequest request,
            CancellationToken cancellationToken = default
        );

        Task<AccountResponse> UpdateAsync(
            int accountId,
            AccountRequest request,
            CancellationToken cancellationToken = default
        );

        Task DeleteAsync(int accountId, CancellationToken cancellationToken = default);
    }
}
