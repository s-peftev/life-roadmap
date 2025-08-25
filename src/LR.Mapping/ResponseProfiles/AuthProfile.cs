using AutoMapper;
using LR.Application.DTOs.Token;
using LR.Application.Responses.User;

namespace LR.Mapping.ResponseProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile() 
        {
            CreateMap<TokenPairDto, AuthResponse>()
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken.TokenValue))
            .ForMember(dest => dest.AccessTokenExpiresAtUtc, opt => opt.MapFrom(src => src.AccessToken.ExpiresAtUtc))
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken.Token))
            .ForMember(dest => dest.RefreshTokenExpiresAtUtc, opt => opt.MapFrom(src => src.RefreshToken.ExpiresAtUtc))
            .ForMember(dest => dest.Message, opt => opt.Ignore());
        }
    }
}
