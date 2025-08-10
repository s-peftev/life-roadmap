using System.ComponentModel.DataAnnotations;

namespace LR.Application.Requests
{
    public class UserRegisterRequest
    {
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string? UserName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

    }
}
