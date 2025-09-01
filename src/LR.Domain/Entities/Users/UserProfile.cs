using System.ComponentModel.DataAnnotations;

namespace LR.Domain.Entities.Users
{
    public class UserProfile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string UserId { get; set; } = null!;
    }
}
