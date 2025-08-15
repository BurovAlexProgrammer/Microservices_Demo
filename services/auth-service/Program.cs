using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    public static void Main(string[] args)
    {
        var usersData = new ConcurrentDictionary<string, UserData>();
        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

// app.UseHttpsRedirection();

        app.MapPost("/login", async (HttpContext context) =>
        {
            var userData = await context.Request.ReadFromJsonAsync<UserData>();

            if (userData == null)
            {
                return Results.BadRequest(new {error = "Неверные данные"});
            }

            if (usersData.TryGetValue(userData.fingerprint, out var currUserData) == false)
            {
                currUserData = new UserData(userData.fingerprint, userData.name);
            }

            return Results.Ok(currUserData);
        });

        app.MapPost("/register", async context =>
        {
            var userData = await context.Request.ReadFromJsonAsync<UserData>();

            if (userData == null)
            {
        
            }
            else
            {
                Results.BadRequest("Данные пользователя повреждены.");
            }
        });

        app.MapGet("/checkAuth", () =>
        {
            return new List<string>();
        });

        app.Run();
    }
}

record UserData(string fingerprint, string name);