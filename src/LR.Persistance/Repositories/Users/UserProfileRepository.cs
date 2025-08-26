using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;

namespace LR.Persistance.Repositories.Users
{
    public class UserProfileRepository(AppDbContext context)
        : Repository<UserProfile, Guid>(context), IUserProfileRepository
    {
    }
}
