using AutoMapper;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.API.Mappers
{
    public class BlogMapper : Profile
    {
        public BlogMapper()
        {
            CreateMap<Blog, BlogResponse>()
                .ForMember(dest => dest.BlogTagName, opt => opt.MapFrom(src => src.BlogTag.Name))
                .ForMember(dest => dest.BlogImageUrl, opt => opt.MapFrom(src => src.BlogImageUrl));

            CreateMap<BlogRequest, Blog>()
                .ForMember(dest => dest.BlogId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.BlogTag, opt => opt.Ignore())
                .ForMember(dest => dest.BlogImageUrl, opt => opt.MapFrom(src => src.BlogImageUrl));
        }
    }
}
