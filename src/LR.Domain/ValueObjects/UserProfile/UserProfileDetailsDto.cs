namespace LR.Domain.ValueObjects.UserProfile
{
    public class UserProfileDetailsDto
    {
        public string UserName { get; }
        public string? FirstName { get; }
        public string? LastName { get; }
        public string? Email { get; }
        public bool IsEmailConfirmed { get; }
        public string? ProfilePhotoUrl { get; }
        public DateTime? BirthDate { get; }

        public UserProfileDetailsDto(string userName, string? firstName, string? lastName, string? email,
            bool isEmailConfirmed, string? profilePhotoUrl, DateTime? birthDate)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsEmailConfirmed = isEmailConfirmed;
            ProfilePhotoUrl = profilePhotoUrl;
            BirthDate = birthDate;
        }
    }
}
