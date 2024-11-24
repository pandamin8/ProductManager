using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Infrastructure.Persistence;

namespace ProductManager.Infrastructure.Seeders;

public class ProductManagerSeeder(
    ProductManagerDbContext dbContext,
    UserManager<User> userManager,
    ILogger<ProductManagerSeeder> logger,
    IConfiguration configuration) : IProductManagerSeeder
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

            var adminUsers = GetAdminUsers();
            await SeedAdminUsers(adminUsers);
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
    
    private async Task SeedAdminUsers(IEnumerable<User> adminUsers)
    {
        foreach (var adminUser in adminUsers)
        {
            var existingUser = await userManager.FindByEmailAsync(adminUser.Email!);

            if (existingUser == null)
            {
                var result = await userManager.CreateAsync(adminUser, adminUser.PasswordHash!);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        logger.LogError(error.Description);
                    }
                }
            }
        }
    }
    
    private IEnumerable<User> GetAdminUsers()
    {
        var adminSettings = configuration.GetSection("AdminSettings").Get<AdminSettings>();

        if (adminSettings == null)
            throw new Exception("Please provide AdminSettings in appsettings.json");
                
        List<User> users =
        [
            new()
            {
                Email = adminSettings.Email,
                UserName = adminSettings.UserName,
                PasswordHash = adminSettings.Password,
                FirstName = adminSettings.FirstName,
                LastName = adminSettings.LastName,
            }
        ];

        return users;
    }
}
