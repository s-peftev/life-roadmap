using LR.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace LR.Persistance.Identity
{
    public class AppUser : IdentityUser
    {
        public UserProfile Profile { get; set; } = null!;
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
