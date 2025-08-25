namespace LR.Infrastructure.Exceptions.Account
{
    public class RegistrationFailedException(string userName)
        : Exception($"Failed to register user: '{userName}'.")
    {
    }
}
