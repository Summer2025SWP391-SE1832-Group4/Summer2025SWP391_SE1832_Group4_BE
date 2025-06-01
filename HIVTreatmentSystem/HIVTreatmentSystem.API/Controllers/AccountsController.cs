using AutoMapper;
using HIVTreatmentSystem.Application.Dtos;
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
        private readonly IMapper _mapper;

        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountsAsync([FromQuery] ListAccountsRequest request)
        {
            ListAccountsResponse response = await _accountService.GetPagedAsync(request);

            var apiDtos = response.Accounts.Select(dto => _mapper.Map<AccountDto>(dto)).ToList();

            return Ok(new { Accounts = apiDtos, TotalCount = response.TotalCount });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountByIdAsync(int id)
        {
            var response = await _accountService.GetByIdAsync(id);
            var apiDto = _mapper.Map<AccountDto>(response.Account);
            return Ok(apiDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] AccountRequest apiRequest)
        {
            var appRequest = _mapper.Map<AccountRequest>(apiRequest);
            var response = await _accountService.UpdateAsync(id, appRequest);
            var apiDto = _mapper.Map<AccountDto>(response.Account);
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
