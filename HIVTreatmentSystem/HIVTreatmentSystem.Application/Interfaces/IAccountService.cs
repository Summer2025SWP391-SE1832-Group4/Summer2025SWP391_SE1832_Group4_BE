using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IAccountService
    {
        Task<(IEnumerable<AccountResponse> Items, int TotalCount)> GetAllAsync(
            string? usernameFilter,
            string? emailFilter,
            AccountStatus? statusFilter,
            int? roleIdFilter,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default
        );

        Task<AccountResponse> GetByIdAsync(
            int accountId,
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
