namespace LR.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static void UseConfiguredSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "LifeRoadmap API v1");
                options.RoutePrefix = string.Empty;
            });
        }
    }
}
