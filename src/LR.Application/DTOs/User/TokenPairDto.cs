using LR.Domain.Entities.Users;

namespace LR.Application.DTOs.User
{
    public class TokenPairDto(AccessTokenDto accessToken, RefreshToken refreshToken)
    {
        public AccessTokenDto AccessToken { get; set; } = accessToken;
        public RefreshToken RefreshToken { get; set; } = refreshToken;
    }
}
