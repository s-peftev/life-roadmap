using LR.Application.Interfaces.Services;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;

namespace LR.Application.Services.User
{
    public class RefreshTokenService(IRefreshTokenRepository repository)
        : EntityService<RefreshToken, Guid>(repository), IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repository = repository;

        public async Task<RefreshToken?> GetByTokenValueAsync(string refreshTokenValue)
        {
            return await _repository.GetByTokenValueAsync(refreshTokenValue);
        }
    }
}
