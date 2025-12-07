namespace LR.Application.Requests.User
{
    public class EmailCodeRequest
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
    }
}
