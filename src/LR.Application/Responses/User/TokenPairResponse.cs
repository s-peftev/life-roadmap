using LR.Application.DTOs.Token;
using LR.Domain.Entities.Users;

namespace LR.Application.Responses.User
{
    public class TokenPairResponse(AccessTokenDto accessToken, RefreshToken refreshToken)
    {
        public AccessTokenDto AccessToken { get; set; } = accessToken;
        public RefreshToken RefreshToken { get; set; } = refreshToken;
    }
}
