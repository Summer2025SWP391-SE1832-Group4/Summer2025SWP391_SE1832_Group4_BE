using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class ScheduledActivityMapper : Profile
    {
        public ScheduledActivityMapper()
        {
            CreateMap<ScheduledActivityRequest, ScheduledActivity>();
            CreateMap<ScheduledActivity, ScheduledActivityResponse>();
        }
    }
}
