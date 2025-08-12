using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LR.Persistance.Repositories.Users
{
    public class RefreshTokenRepository(AppDbContext context)
        : Repository<RefreshToken, Guid>(context), IRefreshTokenRepository
    {
        public async Task<RefreshToken?> GetByTokenValueAsync(string refreshTokenValue)
        {
            return await _dbSet.FirstOrDefaultAsync(rt => rt.Token == refreshTokenValue);
        }
    }
}
