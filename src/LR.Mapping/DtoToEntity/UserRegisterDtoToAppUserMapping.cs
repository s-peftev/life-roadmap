using AutoMapper;
using LR.Application.DTOs.User;
using LR.Persistance.Identity;

namespace LR.Mapping.DtoToEntity
{
    public class UserRegisterDtoToAppUserMapping : Profile
    {
        public UserRegisterDtoToAppUserMapping() 
        {
            CreateMap<UserRegisterDto, AppUser>()
                .ForMember(dest => dest.Profile, opt => opt.Ignore());
        }
    }
}
