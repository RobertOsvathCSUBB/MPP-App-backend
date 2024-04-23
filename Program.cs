using mpp_app_backend.Interfaces;
using mpp_app_backend.Repositories;
using mpp_app_backend;
using mpp_app_backend.Models;
using mpp_app_backend.Services;
using mpp_app_backend.Health;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using mpp_app_backend.Hubs;
using mpp_app_backend.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<Seed>();
builder.Services.AddHealthChecks()
    .AddCheck<InternetHealthCheck>("InternetHealthCheck");
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoginActivityRepository, LoginActivityRepository>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowFrontendOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseLazyLoadingProxies();
});

var app = builder.Build();

if (args.Length == 1 && args[0] == "seeddata")
    SeedDataContext(app);

void SeedDataContext(IHost app)
{
    var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
    using (var scope = scopedFactory.CreateScope())
    {
        var seed = scope.ServiceProvider.GetRequiredService<Seed>();
        seed.SeedDataContext();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontendOrigin");

app.MapHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapHub<DataRefreshHub>("/hub");

app.Run();
