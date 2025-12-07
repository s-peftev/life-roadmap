using LR.Application.AppResult;
using LR.Domain.Entities.Users;

namespace LR.Application.Interfaces.Services
{
    public interface IRefreshTokenService : IEntityService<RefreshToken, Guid>
    {
        Task<Result<RefreshToken>> GetByTokenValueAsync(string refreshTokenValue, CancellationToken ct = default);
    }
}