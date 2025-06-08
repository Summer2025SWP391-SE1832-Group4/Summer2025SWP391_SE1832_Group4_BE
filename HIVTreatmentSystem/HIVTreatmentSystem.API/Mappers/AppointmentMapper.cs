using AutoMapper;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class AppointmentMapper : Profile
    {
        public AppointmentMapper()
        {
            CreateMap<Appointment, AppointmentResponse>();
        }
    }
}
