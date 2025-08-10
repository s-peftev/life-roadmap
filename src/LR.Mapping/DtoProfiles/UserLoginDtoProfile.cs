using AutoMapper;
using LR.Application.DTOs.User;
using LR.Application.Requests;

namespace LR.Mapping.DtoProfiles
{
    public class UserLoginDtoProfile : Profile
    {
        public UserLoginDtoProfile()
        {
            CreateMap<UserLoginRequest, UserLoginDto>();
        }
    }
}
