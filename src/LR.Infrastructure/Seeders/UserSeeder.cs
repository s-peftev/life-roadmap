using LR.Domain.Entities.Users;
using LR.Domain.Enums;
using LR.Persistance;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LR.Infrastructure.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedUsersAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (dbContext.Users.Any())
                return;

            var users = new List<(AppUser User, string Password, string[] Roles)>
            { 
                (
                    new AppUser
                    { 
                        UserName = "admin",
                        Email = "admin@lr.com",
                        EmailConfirmed = true,
                        Profile = new UserProfile
                        { 
                            FirstName = "System",
                            LastName = "Admin"
                        }
                    },
                    "Admin1234",
                    new[] { Role.Admin.ToString() }
                ),

                (
                    new AppUser
                    {
                        UserName = "orin",
                        Email = "orin@lr.com",
                        Profile = new UserProfile
                        {
                            FirstName = "Orin",
                            LastName = "the Red"
                        }
                    },
                    "666Orin666",
                    new[] { Role.User.ToString() }
                ),

                (
                    new AppUser
                    {
                        UserName = "urge",
                        Email = "urge@lr.com",
                        Profile = new UserProfile
                        {
                            FirstName = "Dark",
                            LastName = "Urge"
                        }
                    },
                    "666Urge666",
                    new[] { Role.User.ToString() }
                )
            };

            foreach (var (user, password, roles) in users)
            {
                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create user {user.UserName}: " +
                        $"{string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                var roleResult = await userManager.AddToRolesAsync(user, roles);

                if (!roleResult.Succeeded)
                {
                    throw new Exception($"Failed to assign user role {user.Email}: " +
                        $"{string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
