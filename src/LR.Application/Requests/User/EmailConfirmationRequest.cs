namespace LR.Application.Requests.User
{
    public class EmailConfirmationRequest
    {
        public required string Code { get; set; }
        public required string Email { get; set; }
    }
}
