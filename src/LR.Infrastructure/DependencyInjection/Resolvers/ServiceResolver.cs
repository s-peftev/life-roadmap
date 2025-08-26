using LR.Application.Interfaces.Services;
using LR.Application.Services;
using LR.Application.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace LR.Infrastructure.DependencyInjection.Resolvers
{
    internal class ServiceResolver
    {
        internal static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        }
    }
}
