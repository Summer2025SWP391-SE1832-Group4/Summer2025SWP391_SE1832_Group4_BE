using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class DashboardMapper : Profile
    {
        public DashboardMapper()
        {
            CreateMap<DashboardStatistics, DashboardStatisticsResponse>();
        }
    }
}
