namespace LR.Domain.Exceptions.User
{
    public class UserAlreadyExistsException(string message)
        : Exception(message)
    {
    }
}
