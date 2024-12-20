using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;

namespace ProductManager.Infrastructure.Persistence;

public class ProductManagerDbContext(DbContextOptions<ProductManagerDbContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Product>()
            .OwnsOne(product => product.ManufactureContact, contact =>
            {
                contact.Property(c => c.ManufactureEmail)
                    .HasMaxLength(256)
                    .IsRequired();

                contact.Property(c => c.ManufacturePhone)
                    .HasMaxLength(11)
                    .IsRequired();
                
                contact.HasIndex(c => c.ManufactureEmail)
                    .IsUnique();
                contact.HasIndex(c => c.ManufacturePhone)
                    .IsUnique();
            });

        builder.Entity<User>()
            .HasMany(user => user.Products)
            .WithOne(product => product.User)
            .HasForeignKey(product => product.UserId);
            
        builder.Entity<User>()
            .Property(user => user.FirstName)
            .HasMaxLength(100);
        
        builder.Entity<User>()
            .Property(user => user.LastName)
            .HasMaxLength(100);
    }
}
