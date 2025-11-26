using LR.Infrastructure.Constants;

namespace LR.API.Middleware
{
    public class CorrelationIdMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            const string headerName = HttpHeaders.XCorrelationId;

            if (!context.Request.Headers.TryGetValue(headerName, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            context.Response.Headers[headerName] = correlationId;
            context.Items[headerName] = correlationId;

            await next(context);
        }
    }
}