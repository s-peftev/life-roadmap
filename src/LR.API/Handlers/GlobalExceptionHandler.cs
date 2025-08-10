using LR.Domain.Exceptions.User;
using LR.Infrastructure.Exceptions.Account;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace LR.API.Handlers
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) 
        : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            var (statusCode, message) = GetExceptionDetails(exception);

            _logger.LogError(exception, exception.Message);

            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(message, cancellationToken);

            return true;
        }

        private (HttpStatusCode statusCode, string message) GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                LoginFailedException _ => (HttpStatusCode.Unauthorized, exception.Message),
                LogoutFailedException _ => (HttpStatusCode.InternalServerError, exception.Message),
                UserAlreadyExistsException _ => (HttpStatusCode.Conflict, exception.Message),
                RegistrationFailedException _ => (HttpStatusCode.BadRequest, exception.Message),
                RefreshTokenException _ => (HttpStatusCode.BadRequest, exception.Message),
                AuthenticationTokenException _ => (HttpStatusCode.Unauthorized, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };
        }
    }
}
