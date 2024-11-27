using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
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
        
        var products = new List<Product>
        {
            new Product
            {
                Id = 1, Name = "Product A", ProduceDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                UserId = "1", IsAvailable = true
            },
            new Product
            {
                Id = 2, Name = "Product B", ProduceDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), UserId = "1",
                IsAvailable = true
            },
            new Product
            {
                Id = 3, Name = "Another Product", ProduceDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                UserId = "1", IsAvailable = true
            },
        };

        _dbContext.Products.AddRange(products);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetAllMatchingAsync_WithSearchPhrase_FiltersCorrectly()
    {
        // Act
        var (products, totalCount) =
            await _repository.GetAllMatchingAsync("Product", 10, 1, null, SortDirection.Ascending);

        // Assert
        totalCount.Should().Be(3);
        products.Should().HaveCount(3);
        products.Should().Contain(p => p.Name == "Product A");
        products.Should().Contain(p => p.Name == "Product B");
    }

    [Fact]
    public async Task GetAllMatchingAsync_WithPaging_ReturnsCorrectPage()
    {
        // Act
        var (products, totalCount) = await _repository.GetAllMatchingAsync(null, 2, 1, null, SortDirection.Ascending);

        // Assert
        totalCount.Should().Be(3);
        products.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllMatchingAsync_WithSorting_SortsCorrectly()
    {
        // Act
        var (products, totalCount) =
            await _repository.GetAllMatchingAsync(null, 10, 1, nameof(Product.ProduceDate), SortDirection.Descending);

        // Assert
        totalCount.Should().Be(3);
        products.First().Name.Should().Be("Another Product");
    }

    [Fact]
    public async Task GetAllMatchingAsync_WithNoSearchPhrase_ReturnsAll()
    {
        // Act
        var (products, totalCount) = await _repository.GetAllMatchingAsync(null, 10, 1, null, SortDirection.Ascending);

        // Assert
        totalCount.Should().Be(3);
        products.Should().HaveCount(3);
    }
}
