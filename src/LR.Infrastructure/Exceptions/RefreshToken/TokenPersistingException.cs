namespace LR.Infrastructure.Exceptions.RefreshToken
{
    public class TokenPersistingException : Exception
    {
        public TokenPersistingException()
            : base("Failed to persist the refresh token.") { }

        public TokenPersistingException(string message) : base(message) { }

        public TokenPersistingException(string message, Exception inner) : base(message, inner) { }
    }
}
