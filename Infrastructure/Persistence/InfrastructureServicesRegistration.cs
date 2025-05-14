using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositories;
using Services;
using StackExchange.Redis;

namespace Persistence
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration Configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
           
          );
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            }

         );
            services.AddScoped<IDbInitializer, DbInitializer>();//Allow DI for DbIntilizer
           services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AssemblyReference).Assembly);
            services.AddScoped<IBasketRepository,BasketRepository>();
            services.AddScoped<ICacheRepository,CacheRepository>();
            services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                return ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")!);
            });
            return services;
        }
    }
}
