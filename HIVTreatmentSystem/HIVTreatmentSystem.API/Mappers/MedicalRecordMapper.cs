using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    /// <summary>
    /// AutoMapper profile for Medical Record mappings
    /// </summary>
    public class MedicalRecordMapper : Profile
    {
        public MedicalRecordMapper()
        {
            // Map from Entity to Response
            CreateMap<MedicalRecord, MedicalRecordResponse>()
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Patient))
                .ForMember(dest => dest.Doctor, opt => opt.MapFrom(src => src.Doctor))
                .ForMember(dest => dest.TestResults, opt => opt.MapFrom(src => src.TestResults))
                .ForMember(dest => dest.PatientTreatments, opt => opt.Ignore());

            // Map from Request to Entity
            CreateMap<MedicalRecordRequest, MedicalRecord>();

            // Map create-request to Request & Entity (giúp controller dùng AutoMapper)
            CreateMap<MedicalRecordCreateRequest, MedicalRecordRequest>();
            CreateMap<MedicalRecordCreateRequest, MedicalRecord>();
        }
    }
} 