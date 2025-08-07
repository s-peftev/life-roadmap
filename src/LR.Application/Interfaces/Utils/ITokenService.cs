using LR.Application.DTOs.User;

namespace LR.Application.Interfaces.Utils
{
    public interface ITokenService
    {
        (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(TokenUserDto userDto);
        string GenerateRefreshToken();
        void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
    }
}
