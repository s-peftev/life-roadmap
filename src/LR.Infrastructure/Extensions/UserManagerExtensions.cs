using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LR.Infrastructure.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser?> GetByUserNameWithRolesAsync(this UserManager<AppUser> userManager, string userName)
        {
            return await userManager.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public static async Task<AppUser?> GetByIdWithRolesAsync(this UserManager<AppUser> userManager, string id)
        {
            return await userManager.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
