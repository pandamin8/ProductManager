using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Types;
using ProductManager.Infrastructure.Persistence;
using ProductManager.Infrastructure.Repositories;
using Xunit;

namespace ProductManager.Infrastructure.Tests.Repositories;

[TestSubject(typeof(ProductsRepository))]
public class ProductsRepositoryTest
{
    private readonly ProductManagerDbContext _dbContext;
    private readonly ProductsRepository _repository;

    public ProductsRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<ProductManagerDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new ProductManagerDbContext(options);
        _repository = new ProductsRepository(_dbContext);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        _dbContext.Products.RemoveRange(_dbContext.Products);
        _dbContext.Users.RemoveRange(_dbContext.Users);

        var user1 = new User()
        {
            Id = "1",
            Email = "test1@test.com",
            FirstName = "Test",
            LastName = "Test",
        };
        
        var user2 = new User()
        {
            Id = "2",
            Email = "test2@test.com",
            FirstName = "Test",
            LastName = "Test",
        };
        
        var products = new List<Product>
        {
            new Product
            {
                Id = 1, Name = "Product A", ProduceDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                UserId = "1", IsAvailable = true, User = user1
            },
            new Product
            {
                Id = 2, Name = "Product B", ProduceDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), UserId = "2",
                IsAvailable = true, User = user2
            },
            new Product
            {
                Id = 3, Name = "Product C", ProduceDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                UserId = "1", IsAvailable = true, User = user1
            },
        };

        _dbContext.Products.AddRange(products);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetAllMatchingAsync_WithSearchPhrase_FiltersCorrectly()
    {
        // Act
        var input = new GetAllMatchingProductsInput(
            SearchPhrase: "Product",
            PageSize: 10,
            PageNumber: 1,
            SortBy: null,
            SortDirection: SortDirection.Ascending,
            UserId: null
        );
        
        var (products, totalCount) =
            await _repository.GetAllMatchingAsync(input);

        // Assert
        totalCount.Should().Be(3);
        var enumerable = products as Product[] ?? products.ToArray();
        
        enumerable.Should().HaveCount(3);
        enumerable.Should().Contain(p => p.Name == "Product A");
        enumerable.Should().Contain(p => p.Name == "Product B");
    }

    [Fact]
    public async Task GetAllMatchingAsync_WithPaging_ReturnsCorrectPage()
    {
        // Act
        var input = new GetAllMatchingProductsInput(
            SearchPhrase: null,
            PageSize: 2,
            PageNumber: 1,
            SortBy: null,
            SortDirection: SortDirection.Ascending,
            UserId: null
        );
        
        var (products, totalCount) = await _repository.GetAllMatchingAsync(input);

        // Assert
        totalCount.Should().Be(3);
        products.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllMatchingAsync_WithSorting_SortsCorrectly()
    {
        // Act
        var input = new GetAllMatchingProductsInput(
            SearchPhrase: null,
            PageSize: 10,
            PageNumber: 1,
            SortBy: nameof(Product.ProduceDate),
            SortDirection: SortDirection.Descending,
            UserId: null
        );
        
        var (products, totalCount) =
            await _repository.GetAllMatchingAsync(input);

        // Assert
        totalCount.Should().Be(3);
        products.First().Name.Should().Be("Product C");
    }

    [Fact]
    public async Task GetAllMatchingAsync_WithNoSearchPhrase_ReturnsAll()
    {
        // Act
        var input = new GetAllMatchingProductsInput(
            SearchPhrase: null,
            PageSize: 10,
            PageNumber: 1,
            SortBy: null,
            SortDirection: SortDirection.Descending,
            UserId: null
        );
        
        var (products, totalCount) = await _repository.GetAllMatchingAsync(input);

        // Assert
        totalCount.Should().Be(3);
        products.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task GetAllMatchingAsync_WithUserIdFilter_FiltersCorrectly()
    {
        // Act
        var input = new GetAllMatchingProductsInput(
            SearchPhrase: null,
            PageSize: 10,
            PageNumber: 1,
            SortBy: null,
            SortDirection: SortDirection.Descending,
            UserId: "1"
        );
        
        var (products, totalCount) = await _repository.GetAllMatchingAsync(input);

        // Assert
        totalCount.Should().Be(2);
        var enumerable = products as Product[] ?? products.ToArray();
        
        enumerable.Should().Contain(p => p.Name == "Product A");
        enumerable.Should().Contain(p => p.Name == "Product C");
    } 
}
