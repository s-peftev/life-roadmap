using AutoMapper;
using LR.Application.DTOs.User;
using LR.Persistance.Identity;

namespace LR.Mapping.EntityToDto
{
    public class AppUserToUserDtoMapping : Profile
    {
        public AppUserToUserDtoMapping() 
        {
            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.FirstName, 
                    opt => opt.MapFrom(src => src.Profile.FirstName))
                .ForMember(dest => dest.LastName,
                    opt => opt.MapFrom(src => src.Profile.LastName));
        }
    }
}
