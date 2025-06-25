using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class PatientMapper : Profile
    {
        public PatientMapper()
        {
            CreateMap<Patient, PatientResponse>();
            CreateMap<CreatePatientRequest, Patient>();
        }
    }
}
