using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Dtos;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
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

        public async Task<ListAccountsResponse> GetPagedAsync(
            ListAccountsRequest request,
            CancellationToken cancellationToken = default
        )
        {
            var (entities, totalCount) = await _accountRepository.GetPagedAsync(
                usernameFilter: request.Username,
                emailFilter: request.Email,
                statusFilter: request.AccountStatus,
                roleIdFilter: request.RoleId,
                sortBy: request.SortBy,
                sortDescending: request.SortDescending,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize
            );

            var dtos = entities
                .Select(a => new AccountDto
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
                })
                .ToList();

            return new ListAccountsResponse { Accounts = dtos, TotalCount = totalCount };
        }

        public async Task<AccountResponse> GetByIdAsync(
            int accountId,
            CancellationToken cancellationToken = default
        )
        {
            var a = await _accountRepository.GetByIdAsync(accountId);
            var dto = new AccountDto
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
            return new AccountResponse { Account = dto };
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

            var dto = new AccountDto
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

            return new AccountResponse { Account = dto };
        }

        public async Task DeleteAsync(int accountId, CancellationToken cancellationToken = default)
        {
            var entity = await _accountRepository.GetByIdAsync(accountId);
            _accountRepository.Remove(entity);
        }
    }
}
