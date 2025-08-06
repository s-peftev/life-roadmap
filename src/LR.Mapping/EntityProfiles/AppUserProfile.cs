using AutoMapper;
using LR.Application.DTOs.User;
using LR.Persistance.Identity;

namespace LR.Mapping.EntityProfiles
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile() 
        {
            CreateMap<UserRegisterDto, AppUser>()
                .ForMember(dest => dest.Profile, opt => opt.Ignore());
        }
    }
}
