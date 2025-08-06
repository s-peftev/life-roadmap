using AutoMapper;
using LR.Application.DTOs.User;
using LR.Persistance.Identity;

namespace LR.Mapping.DtoProfiles
{
    public class UserDtoProfile : Profile
    {
        public UserDtoProfile() 
        {
            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.FirstName, 
                    opt => opt.MapFrom(src => src.Profile.FirstName))
                .ForMember(dest => dest.LastName,
                    opt => opt.MapFrom(src => src.Profile.LastName));
        }
    }
}
