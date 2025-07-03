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
            CreateMap<PatientTreatment, PatientTreatmentResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Patient))
                .ForMember(dest => dest.PrescribingDoctor, opt => opt.MapFrom(src => src.PrescribingDoctor))
                .ForMember(dest => dest.Regimen, opt => opt.MapFrom(src => src.Regimen))
                .ForMember(dest => dest.PatientTestResults, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.TestResults : null))
                .ForMember(dest => dest.PatientMedicalRecord, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.MedicalRecord : null));

            CreateMap<PatientTreatmentRequest, PatientTreatment>()
                .ForMember(dest => dest.PatientTreatmentId, opt => opt.Ignore())
                .ForMember(dest => dest.PatientId, opt => opt.Ignore())
                .ForMember(dest => dest.PrescribingDoctorId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore()); // Will be handled separately in service
        }
    }
} 