using LR.Application.Interfaces.Utils;
using LR.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace LR.Infrastructure.DependencyInjection.Resolvers
{
    internal static class UtilsResolver
    {
        internal static void AddUtils(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddSingleton<ITokenService, TokenService>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRefreshTokenCookieWriter, RefreshTokenCookieWriter>();
            services.AddScoped<IRequestInfoService, RequestInfoService>();
        }
    }
}
