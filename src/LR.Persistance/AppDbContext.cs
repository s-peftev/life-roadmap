using LR.Domain.Entities.Users;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LR.Persistance
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) 
        : IdentityDbContext<
            AppUser,
            AppRole,
            string,
            IdentityUserClaim<string>,
            AppUserRole,
            IdentityUserLogin<string>,
            IdentityRoleClaim<string>, 
            IdentityUserToken<string>
        >(options)
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserProfile>(entity =>
            {
                entity
                    .HasOne<AppUser>()
                    .WithOne(u => u.Profile)
                    .HasForeignKey<UserProfile>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<RefreshToken>(entity =>
            {
                entity
                    .HasOne<AppUser>()
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder
                .Entity<AppUser>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<AppRole>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
