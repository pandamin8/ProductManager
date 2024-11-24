using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;

namespace ProductManager.Infrastructure.Persistence;

public class ProductManagerDbContext(DbContextOptions<ProductManagerDbContext> options)
    : IdentityDbContext<User>(options)
{
    internal DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
            .OwnsOne(product => product.ManufactureContact);

        builder.Entity<User>()
            .HasMany(user => user.Products)
            .WithOne(product => product.User)
            .HasForeignKey(product => product.UserId);

    }
}
