namespace LR.Application.Requests.User
{
    public class ChangePersonalInfoRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
