using LR.Infrastructure.Options;
using LR.Persistance;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace LR.Infrastructure.DependencyInjection.Resolvers
{
    internal static class IdentityResolver
    {
        private const int MinPasswordLenght = 8;
        internal static void AddIdentityServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataProtection();

            services
                .AddIdentityCore<AppUser>(opt => 
                {
                    opt.Password.RequireDigit = true;
                    opt.Password.RequireLowercase = true;
                    opt.Password.RequireUppercase = true;
                    opt.Password.RequiredLength = MinPasswordLenght;
                })
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddAuthentication(opt => 
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt => 
                {
                    var jwtOptions = configuration.GetSection(JwtOptions.JwtOptionsKey)
                        .Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));

                    opt.TokenValidationParameters = new TokenValidationParameters 
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                    };

                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["ACCESS_TOKEN"];

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }
    }
}
