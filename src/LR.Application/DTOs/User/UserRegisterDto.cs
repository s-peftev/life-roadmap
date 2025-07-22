using System.ComponentModel.DataAnnotations;

namespace LR.Application.DTOs.User
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string? UserName { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string? Password { get; set; }
    }
}
