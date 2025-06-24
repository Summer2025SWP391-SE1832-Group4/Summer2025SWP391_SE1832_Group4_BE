using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Application.Mappings
{
    /// <summary>
    /// AutoMapper profile for Feedback entity
    /// </summary>
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackResponse>();
            CreateMap<FeedbackRequest, Feedback>();
        }
    }
} 