using LR.Application.DTOs.User;
using LR.Application.Interfaces.Utils;


namespace LR.Infrastructure.Utils
{
    public class TokenService : ITokenService
    {
        public (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(TokenUserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
