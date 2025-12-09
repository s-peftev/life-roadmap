using AutoMapper;
using LR.Application.DTOs.User;
using LR.Persistance.Identity;

namespace LR.Mapping.DtoProfiles
{
    public class UserProfileDetailsProfile : Profile
    {
        public UserProfileDetailsProfile() 
        {
            CreateMap<AppUser, UserProfileDetailsDto>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.IsEmailConfirmed,
                    opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.Profile.FirstName))
                .ForMember(dest => dest.LastName,
                    opt => opt.MapFrom(src => src.Profile.LastName))
                .ForMember(dest => dest.ProfilePhotoUrl,
                    opt => opt.MapFrom(src => src.Profile.ProfilePhotoUrl))
                .ForMember(dest => dest.BirthDate,
                    opt => opt.MapFrom(src => src.Profile.BirthDate));
        }
    }
}
