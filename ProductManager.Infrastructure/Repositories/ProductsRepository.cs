using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Repositories;
using ProductManager.Infrastructure.Persistence;

namespace ProductManager.Infrastructure.Repositories;

public class ProductsRepository(ProductManagerDbContext dbContext) : IProductsRepository
{
    public async Task<(IEnumerable<Product>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize,
        int pageNumber, string? sortBy, SortDirection sortDirection)
    {
        var searchPhraseLower = searchPhrase?.ToLower();

        var baseQuery = dbContext
            .Products
            .Where(r => searchPhraseLower == null || (r.Name.ToLower().Contains(searchPhraseLower)));

        var totalCount = await baseQuery.CountAsync();

        if (sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Product, object>>>
            {
                { nameof(Product.Name), p => p.Name },
                { nameof(Product.ProduceDate), p => p.ProduceDate }
            };
            
            var selectedColumn = columnsSelector[sortBy];
            
            baseQuery =
                sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }
        
        var products = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var product = await dbContext
            .Products
            .FirstOrDefaultAsync(product => product.Id == id);

        return product;
    }

    public async Task<int> CreateAsync(Product product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        return product.Id;
    }

    public async Task<Product?> GetByManufacturerEmail(string email)
    {
        var product = await dbContext
            .Products
            .FirstOrDefaultAsync(product => product.ManufactureContact.ManufactureEmail == email);

        return product;
    }

    public async Task<Product?> GetByManufacturerPhoneNumber(string phoneNumber)
    {
        var product = await dbContext
            .Products
            .FirstOrDefaultAsync(product => product.ManufactureContact.ManufacturePhone == phoneNumber);

        return product;
    }

    public async Task DeleteAsync(Product product)
    {
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
    }

    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}
