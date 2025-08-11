using LR.Application.Interfaces.Services;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;

namespace LR.Application.Services
{
    public class RefreshTokenService(IRefreshTokenRepository repository)
        : EntityService<RefreshToken, Guid>(repository), IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repository = repository;

        public Task<RefreshToken> GetByTokenValueAsync(string refreshTokenValue)
        {
            throw new NotImplementedException();
        }
    }
}
