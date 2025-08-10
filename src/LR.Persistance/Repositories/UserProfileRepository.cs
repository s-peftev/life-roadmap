using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LR.Persistance.Repositories
{
    public class UserProfileRepository(AppDbContext context)
        : Repository<UserProfile, Guid>(context), IUserProfileRepository
    {
        public async Task<UserProfile?> GetByUserProfileByRefreshTokenAsync(string refreshToken)
        {
            var userProfile = await _dbSet.FirstOrDefaultAsync(up => up.RefreshToken == refreshToken);

            return userProfile;
        }
    }
}
