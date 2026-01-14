using LR.Application.Enums.SearchFields;
using LR.Persistance.Identity;
using System.Linq.Expressions;

namespace LR.Persistance.SearchConfig.User
{
    public static class UserSearchConfig
    {
        public static readonly IReadOnlyDictionary<UserSearchField, Expression<Func<AppUser, string?>>> Map =
            new Dictionary<UserSearchField, Expression<Func<AppUser, string?>>>
        {
            { UserSearchField.UserName, u => u.UserName },
            { UserSearchField.Email, u => u.Email },
            { UserSearchField.FirstName, u => u.Profile.FirstName },
            { UserSearchField.LastName, u => u.Profile.LastName }
        };
    }
}
