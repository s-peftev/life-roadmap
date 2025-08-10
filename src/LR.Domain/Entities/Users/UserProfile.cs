using System.ComponentModel.DataAnnotations;

namespace LR.Domain.Entities.Users
{
    public class UserProfile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [StringLength(100)]
        public string? FirstName { get; set; }
        [StringLength(100)]
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAtUtc { get; set; }
        [Required]
        [StringLength(450)]
        public string UserId { get; set; } = null!;
    }
}
