using LR.Application.DTOs.Token;
using LR.Domain.Entities.Users;

namespace LR.Application.Interfaces.Utils
{
    public interface ITokenService
    {
        AccessTokenDto GenerateJwtToken(JwtGenerationDto userDto);
        RefreshToken GenerateRefreshToken(RefreshTokenGenerationDto dto);
    }
}
