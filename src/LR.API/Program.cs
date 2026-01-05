using LR.API.Configurators;
using LR.API.Filters;
using LR.API.Handlers;
using LR.API.Middleware;
using LR.Infrastructure.Constants;
using LR.Infrastructure.DependencyInjection;
using LR.Infrastructure.Seeders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<PaginationNormalizationFilter>();
    options.Filters.Add<FluentValidationActionFilter>();
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.ConfigureCorsPolicy(builder.Configuration);
builder.Services.ConfigurePolicyBasedAuthorization();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

SerilogConfigurator.Configure();
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(Policies.DefaultCorsPolicy);
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseExceptionHandler(_ => { });

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await RoleSeeder.SeedRolesAsync(app.Services);
await UserSeeder.SeedUsersAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseConfiguredSwagger();
}

app.Run();
