using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class AdverseEffectReportMapper : Profile
    {
        public AdverseEffectReportMapper() {
            CreateMap<AdverseEffectReport, AdverseEffectReportResponse>();
            CreateMap<AdverseEffectReportRequest, AdverseEffectReport>();
        }
    }
}
