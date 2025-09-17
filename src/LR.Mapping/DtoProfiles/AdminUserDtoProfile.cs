using AutoMapper;
using LR.Application.DTOs.Admin;
using LR.Domain.Enums;
using LR.Persistance.Identity;

namespace LR.Mapping.DtoProfiles
{
    public class AdminUserDtoProfile : Profile
    {
        public AdminUserDtoProfile() 
        {
            CreateMap<AppUser, UserForAdminDto>()
            .ForMember(dest => dest.IsEmailConfirmed,
                opt => opt.MapFrom(src => src.EmailConfirmed))
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src.Profile.FirstName))
            .ForMember(dest => dest.LastName,
                opt => opt.MapFrom(src => src.Profile.LastName))
            .ForMember(dest => dest.ProfilePhotoUrl,
                opt => opt.MapFrom(src => src.Profile.ProfilePhotoUrl))
            .ForMember(dest => dest.BirthDate,
                opt => opt.MapFrom(src => src.Profile.BirthDate))
            .ForMember(dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.Profile.CreatedAt))
            .ForMember(dest => dest.LastActive,
                opt => opt.MapFrom(src => src.Profile.LastActive))
            .ForMember(dest => dest.Roles,
                opt => opt.MapFrom(src => src.UserRoles
                    .Select(ur => Enum.Parse<Role>(ur.Role.Name!))));
        }
    }
}
