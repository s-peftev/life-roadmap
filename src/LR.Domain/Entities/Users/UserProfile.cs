using LR.Domain.Interfaces;

namespace LR.Domain.Entities.Users
{
    public class UserProfile : IHasTimestamps
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public string? ProfilePhotoPublicId { get; set; }
        public DateOnly? BirthDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string UserId { get; set; } = null!;
    }
}
