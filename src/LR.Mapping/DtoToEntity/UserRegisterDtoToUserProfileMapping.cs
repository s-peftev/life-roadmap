using AutoMapper;
using LR.Application.DTOs.User;
using LR.Domain.Entities.Users;

namespace LR.Mapping.DtoToEntity
{
    public class UserRegisterDtoToUserProfileMapping : Profile
    {
        public UserRegisterDtoToUserProfileMapping() 
        {
            CreateMap<UserRegisterDto, UserProfile>();
        }
    }
}
