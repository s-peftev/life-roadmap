using LR.Application.Interfaces.Utils;
using LR.Infrastructure.Constants;
using Microsoft.AspNetCore.Http;

namespace LR.Infrastructure.Utils
{
    public class RefreshTokenCookieWriter(IHttpContextAccessor httpContextAccessor)
        : IRefreshTokenCookieWriter
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public void Set(string refreshToken, DateTime expires)
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HttpContext is not available.");

            httpContext.Response.Cookies.Append(
                CookieNames.RefreshToken,
                refreshToken, 
                new CookieOptions
                {
                    HttpOnly = true,
                    Expires = expires,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
        }

        public void Delete()
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HttpContext is not available.");

            httpContext.Response.Cookies.Delete(CookieNames.RefreshToken);
        }
    }
}
