using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Enums;
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
        public async Task<IActionResult> GetAll(
            [FromQuery] string? username,
            [FromQuery] string? email,
            [FromQuery] AccountStatus? accountStatus,
            [FromQuery] int? roleId,
            [FromQuery] string? sortBy,
            [FromQuery] bool sortDesc = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20
        )
        {
            var (items, total) = await _accountService.GetAllAsync(
                username,
                email,
                accountStatus,
                roleId,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            return Ok(
                new ApiResponse
                {
                    Success = true,
                    Message = "Danh sách tài khoản",
                    Data = new { Items = items, TotalCount = total },
                }
            );
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
