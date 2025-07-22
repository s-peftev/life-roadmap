using Microsoft.Extensions.DependencyInjection;

namespace LR.Mapping
{
    public static class AutoMapperRegistration
    {
        public static void RegisterProfiles(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperRegistration).Assembly);
        }
    }
}
