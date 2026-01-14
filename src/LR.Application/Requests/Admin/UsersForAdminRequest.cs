using LR.Application.Enums.SearchFields;

namespace LR.Application.Requests.Admin
{
    public class UsersForAdminRequest : PaginatedRequest
    {
        public string? Search { get; init; }
        public IReadOnlyList<UserSearchField>? SearchIn { get; init; }
    }
}
