using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;

namespace LR.Persistance.Repositories
{
    public class RefreshTokenRepository(AppDbContext context)
        : Repository<RefreshToken, Guid>(context), IRefreshTokenRepository
    {
    }
}
