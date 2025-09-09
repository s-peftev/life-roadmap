namespace LR.Application.Requests.User
{
    public class ResetPasswordRequest
    {
        public required string UserId { get; set; }
        public required string Token { get; set; }
        public required string Password { get; set; }

    }
}
