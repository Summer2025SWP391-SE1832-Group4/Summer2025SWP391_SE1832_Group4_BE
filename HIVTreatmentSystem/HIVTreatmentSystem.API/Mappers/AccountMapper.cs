using AutoMapper;
using HIVTreatmentSystem.Application.Dtos; // Corrected namespace
using HIVTreatmentSystem.Application.Models.Requests;

namespace HIVTreatmentSystem.API.Mappers
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            // App-layer DTO → API-layer DTO
            CreateMap<HIVTreatmentSystem.Application.Dtos.AccountDto, AccountDto>();

            // API-layer Request → App-layer AccountRequest
            CreateMap<AccountRequest, Application.Models.Requests.AccountRequest>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash!));
        }
    }
}
