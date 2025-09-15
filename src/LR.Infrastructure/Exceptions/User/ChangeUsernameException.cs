namespace LR.Infrastructure.Exceptions.User
{
    public class ChangeUsernameException : Exception
    {
        public ChangeUsernameException()
            : base("Failed to change username.") { }

        public ChangeUsernameException(string message) : base(message) { }

        public ChangeUsernameException(Exception inner, string? message = "Failed to change username.") : base(message, inner) { }
    }
}
