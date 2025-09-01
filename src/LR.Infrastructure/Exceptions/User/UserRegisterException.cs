namespace LR.Infrastructure.Exceptions.User
{
    public class UserRegisterException : Exception
    {
        public UserRegisterException()
            : base("Failed to register user.") { }

        public UserRegisterException(string message) : base(message) { }

        public UserRegisterException(Exception inner, string? message = "Failed to register user.") : base(message, inner) { }
    }
}
