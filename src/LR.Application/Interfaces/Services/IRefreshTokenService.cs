using LR.Domain.Entities.Users;

namespace LR.Application.Interfaces.Services
{
    public interface IRefreshTokenService : IEntityService<RefreshToken, Guid>
    {
        Task<RefreshToken> GetByTokenValueAsync(string refreshTokenValue);
    }
}
