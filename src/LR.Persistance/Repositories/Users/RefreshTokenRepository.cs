using LR.Domain.Entities.Users;
using LR.Domain.Interfaces;
using LR.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LR.Persistance.Repositories.Users
{
    public class RefreshTokenRepository(AppDbContext context, IUnitOfWork unitOfWork)
        : Repository<RefreshToken, Guid>(context, unitOfWork), IRefreshTokenRepository
    {
        public async Task<RefreshToken?> GetByTokenValueAsync(string refreshTokenValue, CancellationToken ct)
        {
            return await _dbSet.FirstOrDefaultAsync(rt => rt.Token == refreshTokenValue, ct);
        }
    }
}