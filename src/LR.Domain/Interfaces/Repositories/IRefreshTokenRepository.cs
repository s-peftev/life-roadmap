using LR.Domain.Entities.Users;

namespace LR.Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken, Guid>
    {
        Task<RefreshToken?> GetByTokenValueAsync(string refreshTokenValue);
    }
}
