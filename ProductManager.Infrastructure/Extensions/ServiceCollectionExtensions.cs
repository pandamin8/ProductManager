using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Infrastructure.Persistence;

namespace ProductManager.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProductManagerDb");
        services.AddDbContext<ProductManagerDbContext>(options => options.UseSqlServer(connectionString));
    }
}
