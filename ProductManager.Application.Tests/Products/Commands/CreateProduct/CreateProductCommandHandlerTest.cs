using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManager.Application.Products.Commands.CreateProduct;
using ProductManager.Application.Users;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Repositories;
using Xunit;

namespace ProductManager.Application.Tests.Products.Commands.CreateProduct;

[TestSubject(typeof(CreateProductCommandHandler))]
public class CreateProductCommandHandlerTest
{
    Mock<ILogger<CreateProductCommandHandler>> _loggerMock;
    Mock<IMapper> _mapperMock;
    Mock<IProductsRepository> _productsRepositoryMock;
    Mock<IUserContext> _userContextMock;
    CreateProductCommand _command;

    public CreateProductCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<CreateProductCommandHandler>>();
        _mapperMock = new Mock<IMapper>();

        _userContextMock = new Mock<IUserContext>();
        
        _productsRepositoryMock = new Mock<IProductsRepository>();

        _command = new CreateProductCommand()
        {
            Name = "Test",
            ManufactureEmail = "test@test.com",
            ManufacturePhone = "09121111111",
            IsAvailable = true,
            ProduceDate = DateOnly.FromDateTime(DateTime.Now)
        };
    }

    [Fact]
    public async Task Handle_CreateProduct_ReturnProductId()
    {
        // Arrange
        var product = new Product();

        _mapperMock.Setup(m => m.Map<Product>(_command)).Returns(product);

        _productsRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync(1);

        var currentUser = new CurrentUser("1", "test@test.com", []);
        _userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var commandHandler = new CreateProductCommandHandler(_loggerMock.Object, _mapperMock.Object,
            _productsRepositoryMock.Object, _userContextMock.Object);
        
        // Act
        var result = await commandHandler.Handle(_command, CancellationToken.None);
        
        // Assert
        result.Should().Be(1);
        product.UserId.Should().Be("1");
        _productsRepositoryMock.Verify(r => r.CreateAsync(product), Times.Once);
    }
}
