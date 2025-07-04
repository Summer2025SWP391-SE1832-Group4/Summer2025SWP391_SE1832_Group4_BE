using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class PatientTreatmentMapper : Profile
    {
        public PatientTreatmentMapper()
        {
            CreateMap<PatientTreatmentRequest, PatientTreatment>();
            CreateMap<PatientTreatment, PatientTreatmentResponse>();
        }
    }
}
