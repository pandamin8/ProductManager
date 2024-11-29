using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Repositories;
using ProductManager.Domain.Types;
using ProductManager.Infrastructure.Persistence;

namespace ProductManager.Infrastructure.Repositories;

public class ProductsRepository(ProductManagerDbContext dbContext) : IProductsRepository
{
    public async Task<(IEnumerable<Product>, int)> GetAllMatchingAsync(GetAllMatchingProductsInput input)
    {
        var searchPhraseLower = input.SearchPhrase?.ToLower();

        var baseQuery = dbContext
            .Products
            .Include(product => product.User)
            .Where(r => searchPhraseLower == null || (r.Name.ToLower().Contains(searchPhraseLower)))
            .Where(r => input.UserId == null || r.UserId == input.UserId);

        var totalCount = await baseQuery.CountAsync();

        if (input.SortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Product, object>>>
            {
                { nameof(Product.Name), p => p.Name },
                { nameof(Product.ProduceDate), p => p.ProduceDate }
            };
            
            var selectedColumn = columnsSelector[input.SortBy];
            
            baseQuery =
                input.SortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }
        
        var products = await baseQuery
            .Skip(input.PageSize * (input.PageNumber - 1))
            .Take(input.PageSize)
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
