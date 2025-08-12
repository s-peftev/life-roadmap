using LR.Domain.Entities.Users;
using LR.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LR.Persistance.Repositories.Users
{
    public class UserProfileRepository(AppDbContext context)
        : Repository<UserProfile, Guid>(context), IUserProfileRepository
    {
    }
}
