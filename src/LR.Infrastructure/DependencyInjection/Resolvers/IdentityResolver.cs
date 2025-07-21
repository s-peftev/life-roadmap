using LR.Persistance;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace LR.Infrastructure.DependencyInjection.Resolvers
{
    internal static class IdentityResolver
    {
        internal static void AddIdentityServices(IServiceCollection services)
        {
            services
                .AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
