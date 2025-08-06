using LR.Persistance;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace LR.Infrastructure.DependencyInjection.Resolvers
{
    internal static class IdentityResolver
    {
        private const int MIN_PASSWORD_LENGTH = 8;
        internal static void AddIdentityServices(IServiceCollection services)
        {
            services.AddDataProtection();

            services
                .AddIdentityCore<AppUser>(opt => 
                {
                    opt.Password.RequireDigit = true;
                    opt.Password.RequireLowercase = true;
                    opt.Password.RequireUppercase = true;
                    opt.Password.RequiredLength = MIN_PASSWORD_LENGTH;
                })
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
