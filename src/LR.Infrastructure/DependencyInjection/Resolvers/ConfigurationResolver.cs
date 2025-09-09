using LR.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LR.Infrastructure.DependencyInjection.Resolvers
{
    internal static class ConfigurationResolver
    {
        internal static void AddConfigurationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            services.Configure<RefreshTokenOptions>(configuration.GetSection("RefreshTokenOptions"));
            services.Configure<FrontendOptions>(configuration.GetSection("Frontend"));
        }
    }
}
