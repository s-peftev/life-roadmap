using Microsoft.AspNetCore.Identity;

namespace LR.Persistance.Identity
{
    public class AppRole : IdentityRole<string>
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = [];

        public AppRole() : base() { }

        public AppRole(string roleName) : base(roleName) 
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
