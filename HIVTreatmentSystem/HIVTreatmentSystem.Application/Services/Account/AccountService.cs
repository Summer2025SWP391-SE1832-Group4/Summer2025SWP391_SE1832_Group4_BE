using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<(IEnumerable<AccountResponse> Items, int TotalCount)> GetAllAsync(
            string? usernameFilter,
            string? emailFilter,
            AccountStatus? statusFilter,
            int? roleIdFilter,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default
        )
        {
            var (entities, total) = await _accountRepository.GetPagedAsync(
                usernameFilter,
                emailFilter,
                statusFilter,
                roleIdFilter,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );

            var items = entities.Select(a => new AccountResponse
            {
                AccountId = a.AccountId,
                Username = a.Username,
                Email = a.Email,
                FullName = a.FullName,
                AccountStatus = a.AccountStatus,
                RoleId = a.RoleId,
                CreatedAt = a.CreatedAt,
                LastLoginAt = a.LastLoginAt,
                PhoneNumber = a.PhoneNumber,
                ProfileImageUrl = a.ProfileImageUrl,
            });

            return (items, total);
        }

        public async Task<AccountResponse> GetByIdAsync(
            int accountId,
            CancellationToken cancellationToken = default
        )
        {
            var a = await _accountRepository.GetByIdAsync(accountId);

            return new AccountResponse
            {
                AccountId = a.AccountId,
                Username = a.Username,
                Email = a.Email,
                FullName = a.FullName,
                AccountStatus = a.AccountStatus,
                RoleId = a.RoleId,
                CreatedAt = a.CreatedAt,
                LastLoginAt = a.LastLoginAt,
                PhoneNumber = a.PhoneNumber,
                ProfileImageUrl = a.ProfileImageUrl,
            };
        }

        public async Task<AccountResponse> UpdateAsync(
            int accountId,
            AccountRequest request,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _accountRepository.GetByIdAsync(accountId);

            entity.Username = request.Username;
            if (!string.IsNullOrWhiteSpace(request.PasswordHash))
                entity.PasswordHash = request.PasswordHash!;
            entity.Email = request.Email;
            entity.PhoneNumber = request.PhoneNumber;
            entity.FullName = request.FullName;
            entity.AccountStatus = request.AccountStatus;
            entity.RoleId = request.RoleId;
            entity.ProfileImageUrl = request.ProfileImageUrl;

            _accountRepository.Update(entity);

            return new AccountResponse
            {
                AccountId = entity.AccountId,
                Username = entity.Username,
                Email = entity.Email,
                FullName = entity.FullName,
                AccountStatus = entity.AccountStatus,
                RoleId = entity.RoleId,
                CreatedAt = entity.CreatedAt,
                LastLoginAt = entity.LastLoginAt,
                PhoneNumber = entity.PhoneNumber,
                ProfileImageUrl = entity.ProfileImageUrl,
            };
        }

        public async Task DeleteAsync(int accountId, CancellationToken cancellationToken = default)
        {
            var entity = await _accountRepository.GetByIdAsync(accountId);
            _accountRepository.Remove(entity);
        }
    }
}
