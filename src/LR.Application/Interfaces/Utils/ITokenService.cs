using LR.Application.DTOs.User;
using LR.Domain.Entities.Users;

namespace LR.Application.Interfaces.Utils
{
    public interface ITokenService
    {
        (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(TokenUserDto userDto);
        RefreshToken GenerateRefreshToken(string userId, int ExpirationTimeInDays);
        void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
    }
}
