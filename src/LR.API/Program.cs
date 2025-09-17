using LR.API.Extensions;
using LR.API.Handlers;
using LR.API.Middleware;
using LR.Infrastructure.Constants;
using LR.Infrastructure.DependencyInjection;
using LR.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.ConfigureCorsPolicy(builder.Configuration);
builder.Services.ConfigurePolicyBasedAuthorization();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

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
