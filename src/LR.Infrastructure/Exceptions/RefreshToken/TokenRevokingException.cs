namespace LR.Infrastructure.Exceptions.RefreshToken
{
    public class TokenRevokingException : Exception
    {
        public TokenRevokingException()
            : base("Failed to revoke the refresh token.") { }

        public TokenRevokingException(string message) : base(message) { }

        public TokenRevokingException(string message, Exception inner) : base(message, inner) { }
    }
}
