namespace LR.API.Configurators
{
    public static class SwaggerConfigurator
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
