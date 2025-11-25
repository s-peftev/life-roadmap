using AutoMapper;
using LR.Application.DTOs.User;
using LR.Application.Requests.User;

namespace LR.Mapping.DtoProfiles
{
    public class UserRegisterDtoProfile : Profile
    {
        public UserRegisterDtoProfile()
        {
            CreateMap<UserRegisterRequest, UserRegisterDto>()
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.Email) ? null : src.Email));
        }
    }
}
