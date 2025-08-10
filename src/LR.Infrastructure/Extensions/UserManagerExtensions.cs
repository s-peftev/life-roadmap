using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LR.Infrastructure.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser?> GetByUserNameWithProfileAndRolesAsync(
            this UserManager<AppUser> userManager,
            string userName)
        {
            return await userManager.Users
                .Include(u => u.Profile)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public static async Task<AppUser?> GetByIdWithProfileAsync(
            this UserManager<AppUser> userManager,
            string userId)
        {
            return await userManager.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
