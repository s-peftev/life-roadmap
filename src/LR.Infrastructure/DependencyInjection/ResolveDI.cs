using LR.Application.Validators;
using LR.Domain.Enums;
using LR.Infrastructure.Constants;
using LR.Infrastructure.DependencyInjection.Resolvers;
using LR.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LR.Infrastructure.DependencyInjection
{
    public static class ResolveDI
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;

            PersistanceResolver.AddPersistance(services, connectionString);
            IdentityResolver.AddIdentityServices(services, configuration);
            UtilsResolver.AddUtils(services);
            ExternalProvidersResolver.AddExternalProviders(services);
            ServiceResolver.AddServices(services);
            AutoMapperRegistration.RegisterProfiles(services);
            ValidatorsRegistration.RegisterValidators(services);
            SwaggerResolver.AddSwagger(services);
            ConfigurationResolver.AddConfigurationServices(services, configuration);
        }

        public static void ConfigureCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("FrontEnd:Url").Value;
            services.AddCors(options =>
            {
                options.AddPolicy(Policies.DefaultCorsPolicy, policy =>
                {
                    policy.WithOrigins(allowedOrigins!)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); ;
                });
            });
        }

        public static void ConfigurePolicyBasedAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.RequireAdministratorRole, policy => policy.RequireRole(Role.Admin.ToString()));
            });
        }
    }
}
