using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class BlogTagMapper : Profile
    {
        public BlogTagMapper()
        {
            CreateMap<BlogTag, BlogTagResponse>();
            CreateMap<BlogTagRequest, BlogTag>()
                .ForMember(dest => dest.BlogTagId, opt => opt.Ignore())
                .ForMember(dest => dest.Blogs, opt => opt.Ignore());
        }
    }
}
