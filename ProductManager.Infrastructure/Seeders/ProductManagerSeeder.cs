using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Infrastructure.Persistence;

namespace ProductManager.Infrastructure.Seeders;

public class ProductManagerSeeder(
    ProductManagerDbContext dbContext,
    // UserManager<User> userManager,
    ILogger<ProductManagerSeeder> logger) : IProductManagerSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }
    
    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles =
        [
            new(UserRoles.User)
            {
                NormalizedName = UserRoles.User.ToUpper()
            },
            new(UserRoles.Admin)
            {
                NormalizedName = UserRoles.Admin.ToUpper()
            }
        ];
    
        return roles;
    }
}
