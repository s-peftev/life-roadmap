using LR.Application.AppResult;
using LR.Application.AppResult.Errors;
using LR.Application.Interfaces.Services;
using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;

namespace LR.Application.Services.User
{
    public class RefreshTokenService(IRefreshTokenRepository repository) : EntityService<RefreshToken, Guid>(repository), IRefreshTokenService
    {
        public async Task<Result<RefreshToken>> GetByTokenValueAsync(string refreshTokenValue, CancellationToken ct = default)
        {
            var rt = await repository.GetByTokenValueAsync(refreshTokenValue, ct);

            return rt is null
                ? Result<RefreshToken>.Failure(GeneralErrors.NotFound)
                : Result<RefreshToken>.Success(rt);
        }
    }
}
