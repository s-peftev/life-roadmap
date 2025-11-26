namespace LR.Domain.ValueObjects.UserProfile
{
    public class UserProfileDetailsDto(
        string userName,
        string? firstName,
        string? lastName,
        string? email,
        bool isEmailConfirmed,
        string? profilePhotoUrl,
        DateOnly? birthDate)
    {
        public string UserName { get; } = userName;
        public string? FirstName { get; } = firstName;
        public string? LastName { get; } = lastName;
        public string? Email { get; } = email;
        public bool IsEmailConfirmed { get; } = isEmailConfirmed;
        public string? ProfilePhotoUrl { get; } = profilePhotoUrl;
        public DateOnly? BirthDate { get; } = birthDate;
    }
}
