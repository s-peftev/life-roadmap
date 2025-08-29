using LR.Domain.Entities.Users;
using LR.Domain.Interfaces;
using LR.Domain.Interfaces.Repositories;

namespace LR.Persistance.Repositories.Users
{
    public class UserProfileRepository(AppDbContext context, IUnitOfWork unitOfWork)
        : Repository<UserProfile, Guid>(context, unitOfWork), IUserProfileRepository
    {

    }
}
