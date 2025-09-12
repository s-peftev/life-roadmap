namespace LR.Application.Exceptions.UserProfile
{
    public class ProfilePersistingException : Exception
    {
        public ProfilePersistingException()
            : base("Failed to persist the profile.") { }

        public ProfilePersistingException(string message) : base(message) { }

        public ProfilePersistingException(string message, Exception inner) : base(message, inner) { }
    }
}
