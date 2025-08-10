using AutoMapper;
using LR.Application.DTOs.User;
using LR.Application.Requests;

namespace LR.Mapping.DtoProfiles
{
    public class UserRegisterDtoProfile : Profile
    {
        public UserRegisterDtoProfile() 
        {
            CreateMap<UserRegisterRequest, UserRegisterDto>();
        }
    }
}
