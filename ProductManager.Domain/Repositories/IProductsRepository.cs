using ProductManager.Domain.Entities;

namespace ProductManager.Domain.Repositories;

public interface IProductsRepository
{
    Task<(IEnumerable<Product>, int)> GetAllMatchingAsync(string? searchPhrase, int pageNumber, int pageSize);
    Task<Product?> GetByIdAsync(int id);
    Task<int> CreateAsync(Product product);
    Task<Product?> GetByManufacturerEmail(string email);
    Task<Product?> GetByManufacturerPhoneNumber(string phoneNumber);
    Task DeleteAsync(Product product);
    Task SaveChangesAsync();
}
