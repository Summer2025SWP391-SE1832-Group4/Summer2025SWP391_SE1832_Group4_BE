using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Application.Mappings
{
    /// <summary>
    /// AutoMapper profile for patient treatment mapping
    /// </summary>
    public class PatientTreatmentProfile : Profile
    {
        public PatientTreatmentProfile()
        {
            CreateMap<PatientTreatment, PatientTreatmentResponse>();
            CreateMap<PatientTreatmentRequest, PatientTreatment>()
                .ForMember(dest => dest.PatientTreatmentId, opt => opt.Ignore())
                .ForMember(dest => dest.PatientId, opt => opt.Ignore())
                .ForMember(dest => dest.PrescribingDoctorId, opt => opt.Ignore());
        }
    }
} 