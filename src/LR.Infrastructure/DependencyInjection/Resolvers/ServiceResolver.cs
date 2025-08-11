using LR.Application.Interfaces.Services;
using LR.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LR.Infrastructure.DependencyInjection.Resolvers
{
    internal class ServiceResolver
    {
        internal static void AddServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IEntityService<,>), typeof(EntityService<,>));
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        }
    }
}
