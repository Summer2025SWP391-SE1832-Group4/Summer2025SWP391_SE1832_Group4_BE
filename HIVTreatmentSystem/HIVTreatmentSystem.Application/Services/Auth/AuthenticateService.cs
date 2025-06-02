using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Domain.Interfaces;

namespace HIVTreatmentSystem.Application.Services.Auth
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticateService(
            IAccountRepository accountRepository,
            IPasswordHasher passwordHasher
        )
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> ChangePassword(string oldPassword, string newPassword, int id)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(id);
                bool checkPassword = _passwordHasher.VerifyPassword(
                    oldPassword,
                    account.PasswordHash
                );
                if (!checkPassword)
                    return false;
                account.PasswordHash = _passwordHasher.HashPassword(newPassword);
                _accountRepository.Update(account);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
