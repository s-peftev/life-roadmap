namespace LR.Infrastructure.Exceptions.Account
{
    public class LogoutFailedException(string message)
        : Exception(message)
    {
    }
}
