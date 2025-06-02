using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountsAsync([FromQuery] ListAccountsRequest request)
        {
            ListAccountsResponse response = await _accountService.GetPagedAsync(request);

            var apiDtos = response
                .Accounts.Select(a => new AccountResponse
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

            return Ok(new { Accounts = apiDtos, TotalCount = response.TotalCount });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountByIdAsync(int id)
        {
            var a = await _accountService.GetByIdAsync(id);
            var apiDto = new AccountResponse
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
            return Ok(apiDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] AccountRequest apiRequest)
        {
            var appRequest = new AccountRequest
            {
                Username = apiRequest.Username,
                PasswordHash = apiRequest.PasswordHash,
                Email = apiRequest.Email,
                PhoneNumber = apiRequest.PhoneNumber,
                FullName = apiRequest.FullName,
                AccountStatus = apiRequest.AccountStatus,
                RoleId = apiRequest.RoleId,
                ProfileImageUrl = apiRequest.ProfileImageUrl,
            };

            var a = await _accountService.UpdateAsync(id, appRequest);

            var apiDto = new AccountResponse
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

            return Ok(apiDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _accountService.DeleteAsync(id);
            return NoContent();
        }
    }
}
