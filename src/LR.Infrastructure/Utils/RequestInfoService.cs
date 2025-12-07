using LR.Application.Interfaces.Utils;
using LR.Infrastructure.Constants;
using Microsoft.AspNetCore.Http;

namespace LR.Infrastructure.Utils
{
    public class RequestInfoService(IHttpContextAccessor httpContextAccessor) 
        : IRequestInfoService
    {
        public string? GetIpAddress()
        {
            var context = httpContextAccessor.HttpContext;
            if (context is null) 
                return null;

            var forwarded = context.Request.Headers[HttpHeaders.XForwardedFor].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwarded))
                return forwarded.Split(',').First().Trim();

            return context.Connection.RemoteIpAddress?.ToString();
        }

        public string? GetUserAgent()
        {
            var context = httpContextAccessor.HttpContext;
            if (context is null)
                return null;

            return context.Request.Headers[HttpHeaders.UserAgent].FirstOrDefault();
        }
    }
}
