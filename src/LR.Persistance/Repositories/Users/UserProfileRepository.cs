using LR.Domain.Entities.Users;
using LR.Domain.Interfaces;
using LR.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LR.Persistance.Repositories.Users
{
    public class UserProfileRepository(AppDbContext context, IUnitOfWork unitOfWork)
        : Repository<UserProfile, Guid>(context, unitOfWork), IUserProfileRepository
    {
        public async Task<UserProfile?> GetByUserIdAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(up => up.UserId == userId, cancellationToken);
        }
    }
}
