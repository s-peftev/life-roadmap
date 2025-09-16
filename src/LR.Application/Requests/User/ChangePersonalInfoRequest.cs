namespace LR.Application.Requests.User
{
    public class ChangePersonalInfoRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? BirthDate { get; set; }
    }
}
