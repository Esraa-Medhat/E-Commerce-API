
using Domain.Contracts;
using E_Commerce_API.Extensions;
using E_Commerce_API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistence;
using Persistence.Data;
using Services;
using Services.Abstractions;
using Shared.ErrorsModels;

namespace E_Commerce_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.RegisterAllServices(builder.Configuration);
        


            var app = builder.Build();

           await app.ConfigureMiddlewares();

            app.Run();
        }
    }
}
