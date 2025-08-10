using LR.Domain.Entities.Users;

namespace LR.Domain.Interfaces.Repositories
{
    public interface IUserProfileRepository : IRepository<UserProfile, Guid>
    {
        Task<UserProfile?> GetByUserProfileByRefreshTokenAsync(string refreshToken);
    }
}
