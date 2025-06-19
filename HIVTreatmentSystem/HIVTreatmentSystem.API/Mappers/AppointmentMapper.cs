using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class AppointmentMapper : Profile
    {
        public AppointmentMapper()
        {
            CreateMap<Appointment, AppointmentResponse>()
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Account.FullName : null))
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Account.FullName : null));
            CreateMap<AppointmentRequest, Appointment>();
            CreateMap<AppointmentByDoctorRequest, Appointment>();



        }
    }
}
