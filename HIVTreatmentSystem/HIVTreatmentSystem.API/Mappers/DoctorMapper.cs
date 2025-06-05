using AutoMapper;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class DoctorMapper : Profile
    {
        public DoctorMapper() {
                CreateMap<Doctor, DoctorResponse>();
        }
    }
}
