using AutoMapper;
using LR.Application.DTOs.User;
using LR.Domain.Entities.Users;

namespace LR.Mapping.EntityProfiles
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile() 
        {
            CreateMap<UserRegisterDto, UserProfile>();
        }
    }
}
