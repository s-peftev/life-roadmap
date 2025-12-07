using LR.Domain.Enums;

namespace LR.Domain.ValueObjects.UserProfile
{
    public class UserProfileForAdminDto(
        string id,
        string userName,
        string? firstName,
        string? lastName,
        string? email,
        bool isEmailConfirmed,
        string? profilePhotoUrl,
        DateOnly? birthDate,
        DateTime createdAt,
        DateTime lastActive,
        ICollection<Role> roles)
    {
        public string Id { get; } = id;
        public string UserName { get; } = userName;
        public string? Email { get; } = email;
        public bool IsEmailConfirmed { get; } = isEmailConfirmed;
        public string? FirstName { get; } = firstName;
        public string? LastName { get; } = lastName;
        public string? ProfilePhotoUrl { get; } = profilePhotoUrl;
        public DateOnly? BirthDate { get; } = birthDate;
        public DateTime CreatedAt { get; } = createdAt;
        public DateTime LastActive { get; } = lastActive;
        public ICollection<Role> Roles { get; } = roles;
    }
}
