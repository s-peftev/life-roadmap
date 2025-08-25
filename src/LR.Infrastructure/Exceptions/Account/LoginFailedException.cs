namespace LR.Infrastructure.Exceptions.Account
{
    public class LoginFailedException(string userName)
        : Exception($"Invalid login: '{userName}', or password.")
    {
    }
}
