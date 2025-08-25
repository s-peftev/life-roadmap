namespace LR.Infrastructure.Exceptions.Account
{
    public class AuthenticationTokenException(string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
    }
}
