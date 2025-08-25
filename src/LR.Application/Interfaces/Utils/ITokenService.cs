using LR.Application.DTOs.Token;
using LR.Domain.Entities.Users;

namespace LR.Application.Interfaces.Utils
{
    public interface ITokenService
    {
        AccessTokenDto GenerateJwtToken(TokenUserDto userDto);
        RefreshToken GenerateRefreshToken(string userId, int ExpirationTimeInDays);
        void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
    }
}
