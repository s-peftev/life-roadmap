using LR.Domain.Entities.Users;
using LR.Domain.Interfaces;
using LR.Domain.Interfaces.Repositories;
using LR.Domain.ValueObjects.UserProfile;
using Microsoft.EntityFrameworkCore;

namespace LR.Persistance.Repositories.Users
{
    public class UserProfileRepository(AppDbContext context, IUnitOfWork unitOfWork)
        : Repository<UserProfile, Guid>(context, unitOfWork), IUserProfileRepository
    {
        public async Task<UserProfile?> GetByUserIdAsync(string userId, CancellationToken ct = default)
        {
            return await _dbSet.FirstOrDefaultAsync(up => up.UserId == userId, ct);
        }

        public async Task<UserProfileDetailsDto?> GetProfileProfileDetailsAsync(string userId, CancellationToken ct = default)
        {
            return await (
                from p in _dbSet
                join u in _context.Users on p.UserId equals u.Id
                where p.UserId == userId
                select new UserProfileDetailsDto(
                    u.UserName!,
                    p.FirstName,
                    p.LastName,
                    u.Email,
                    u.EmailConfirmed,
                    p.ProfilePhotoUrl,
                    p.BirthDate
                )
            ).FirstOrDefaultAsync(ct);
        }
    }
}