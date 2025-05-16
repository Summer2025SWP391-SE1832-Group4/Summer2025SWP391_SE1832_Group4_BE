using AutoMapper;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Application.DTOs;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Application.Usecases.Login;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Application.MapperProfiles
{
    /// <summary>
    /// Login mapper profile.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class LoginMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginMapperProfile"/> class.
        /// </summary>
        public LoginMapperProfile()
        {
            CreateMap<LoginCommand, LoginDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ReverseMap();
        }
    }
}
