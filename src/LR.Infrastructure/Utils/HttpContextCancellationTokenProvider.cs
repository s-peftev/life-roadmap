using LR.Application.Interfaces.Utils;
using Microsoft.AspNetCore.Http;

namespace LR.Infrastructure.Utils
{
    public class HttpContextCancellationTokenProvider(IHttpContextAccessor httpContextAccessor) 
        : ICancellationTokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public CancellationToken GetCancellationToken() =>
            _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
    }
}
