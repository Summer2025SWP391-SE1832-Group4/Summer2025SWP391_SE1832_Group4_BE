using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class StandardARVRegimenMapper : Profile
    {
        public StandardARVRegimenMapper()
        {
            CreateMap<StandardARVRegimen, StandardARVRegimenResponse>();
            CreateMap<StandardARVRegimenRequest, StandardARVRegimen>()
                .ForMember(dest => dest.RegimenId, opt => opt.Ignore())
                .ForMember(dest => dest.PatientTreatments, opt => opt.Ignore());
        }
    }
}
