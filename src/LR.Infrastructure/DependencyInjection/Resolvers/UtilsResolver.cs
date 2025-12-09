using LR.Application.Interfaces.Utils;
using LR.Infrastructure.EF.Interceptors;
using LR.Infrastructure.ModelBinding.Pagination;
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
            services.AddSingleton<IErrorResponseFactory, ErrorResponseFactory>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<PaginatedRequestBinder>();

            services.AddScoped<TimestampInterceptor>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IRefreshTokenCookieWriter, RefreshTokenCookieWriter>();
            services.AddScoped<IRequestInfoService, RequestInfoService>();
        }
    }
}
