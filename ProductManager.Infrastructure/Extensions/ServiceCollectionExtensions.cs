using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Authorization.Services;
using ProductManager.Infrastructure.Persistence;
using ProductManager.Infrastructure.Seeders;

namespace ProductManager.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProductManagerDb");
        services.AddDbContext<ProductManagerDbContext>(options => options.UseSqlServer(connectionString));
        
        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ProductManagerDbContext>();
        
        services.AddScoped<IProductManagerSeeder, ProductManagerSeeder>();
        services.AddScoped<IProductAuthorizationService, ProductAuthorizationService>();
    }
}
