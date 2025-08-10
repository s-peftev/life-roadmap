using System.ComponentModel.DataAnnotations;

namespace LR.Application.Requests
{
    public class UserLoginRequest
    {
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string? UserName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string? Password { get; set; }
    }
}
