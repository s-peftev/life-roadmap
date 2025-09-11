using LR.Domain.Entities.Users;

namespace LR.Domain.Interfaces.Repositories
{
    public interface IUserProfileRepository : IRepository<UserProfile, Guid>
    {
        Task<UserProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
