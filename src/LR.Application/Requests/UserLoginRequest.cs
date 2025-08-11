namespace LR.Application.Requests
{
    public class UserLoginRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
