using AutoMapper;
using HIVTreatmentSystem.Application.Dtos;
using HIVTreatmentSystem.Application.Models.Requests;

namespace HIVTreatmentSystem.API.Mappers
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<AccountDto, AccountDto>();
            CreateMap<AccountRequest, AccountRequest>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash!));
        }
    }
}
