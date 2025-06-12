using AutoMapper;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class DoctorMapper : Profile
    {
        public DoctorMapper() {
            CreateMap<Doctor, DoctorResponse>()
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialty))
                .ForMember(dest => dest.Qualifications, opt => opt.MapFrom(src => src.Qualifications))
                .ForMember(dest => dest.YearsOfExperience, opt => opt.MapFrom(src => src.YearsOfExperience))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription));
        }
    }
}
