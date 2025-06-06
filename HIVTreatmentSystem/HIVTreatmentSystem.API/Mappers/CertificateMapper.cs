using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class CertificateMapper : Profile
    {
        public CertificateMapper()
        {
            CreateMap<Certificate, CertificateResponse>();
            CreateMap<CertificateRequest, Certificate>()
                    .ForMember(dest => dest.CertificateId, opt => opt.Ignore());

        }
    }
}
