namespace LR.Application.Requests.User
{
    public class UserLoginRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
