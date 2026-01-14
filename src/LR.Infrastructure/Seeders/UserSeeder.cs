using Bogus;
using LR.Domain.Entities.Users;
using LR.Domain.Enums;
using LR.Infrastructure.Options;
using LR.Persistance;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LR.Infrastructure.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedUsersAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var seedOptions = scope.ServiceProvider.GetRequiredService<IOptions<SeedOptions>>().Value;

            var usersCount = dbContext.Users.Count();

            if (usersCount == 0)
            {
                var admin = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@lr.com",
                    EmailConfirmed = true,
                    Profile = new UserProfile
                    {
                        FirstName = "System",
                        LastName = "Admin"
                    }
                };

                var result = await userManager.CreateAsync(admin, "Admin1234");
                if (!result.Succeeded)
                    throw new Exception($"Failed to create admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");

                var roleResult = await userManager.AddToRoleAsync(admin, Role.Admin.ToString());
                if (!roleResult.Succeeded)
                    throw new Exception($"Failed to assign admin role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");

                usersCount = 1;
            }

            if (seedOptions.EnableFakeUsers && usersCount < seedOptions.FakeUsersCount)
            {
                var toCreate = seedOptions.FakeUsersCount - usersCount;

                var faker = new Faker<AppUser>()
                    .RuleFor(u => u.UserName, f => f.Internet.UserName())
                    .RuleFor(u => u.Email, f => f.Internet.Email())
                    .RuleFor(u => u.EmailConfirmed, true)
                    .RuleFor(u => u.Profile, f => new UserProfile
                    {
                        FirstName = f.Name.FirstName(),
                        LastName = f.Name.LastName()
                    });

                for (int i = 0; i < toCreate; i++)
                {
                    var fakeUser = faker.Generate();

                    var result = await userManager.CreateAsync(fakeUser, "User1234");
                    if (!result.Succeeded)
                        throw new Exception($"Failed to create user {fakeUser.UserName}: " +
                            $"{string.Join(", ", result.Errors.Select(e => e.Description))}");

                    var roleResult = await userManager.AddToRoleAsync(fakeUser, Role.User.ToString());
                    if (!roleResult.Succeeded)
                        throw new Exception($"Failed to assign user role {fakeUser.Email}: " +
                            $"{string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
