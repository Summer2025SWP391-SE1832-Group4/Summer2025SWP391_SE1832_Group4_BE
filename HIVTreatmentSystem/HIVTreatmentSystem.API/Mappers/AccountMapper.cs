using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<AccountRequest, AccountRequest>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash!));
            CreateMap<Account, AccountResponse>();
        }
    }
}
