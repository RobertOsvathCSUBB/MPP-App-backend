using mpp_app_backend.Interfaces;
using mpp_app_backend.Repositories;
using mpp_app_backend;
using mpp_app_backend.Models;
using mpp_app_backend.Services;
using mpp_app_backend.Health;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.WebSockets;
using mpp_app_backend.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHealthChecks()
    .AddCheck<InternetHealthCheck>("InternetHealthCheck");
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<ICollection<User>>(new List<User>());
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<UserServices>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

Seed.SeedUsers(builder.Services);

var app = builder.Build();

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

app.MapHub<DataRefreshHub>("/hub");

app.Run();
