using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManager.Application.Products.Commands.EditProduct;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;
using ProductManager.Domain.Interfaces;
using ProductManager.Domain.Repositories;
using Xunit;

namespace ProductManager.Application.Tests.Products.Commands.EditProduct;

[TestSubject(typeof(EditProductCommandHandler))]
public class EditProductCommandHandlerTest
{
    private readonly Mock<ILogger<EditProductCommandHandler>> _loggerMock;
    private readonly Mock<IProductsRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IProductAuthorizationService> _authServiceMock;
    private readonly EditProductCommandHandler _handler;

    public EditProductCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<EditProductCommandHandler>>();
        _productRepositoryMock = new Mock<IProductsRepository>();
        _mapperMock = new Mock<IMapper>();
        _authServiceMock = new Mock<IProductAuthorizationService>();

        _handler = new EditProductCommandHandler(
            _loggerMock.Object,
            _productRepositoryMock.Object,
            _mapperMock.Object,
            _authServiceMock.Object
        );
    }
    
    [Fact]
    public async Task Handle_ForValidRequest_UpdatesProductSuccessfully()
    {
        // Arrange
        var command = new EditProductCommand
        {
            Id = 1,
            Name = "Updated Product Name"
        };
        var product = new Product { Id = 1, Name = "Original Product Name" };

        _productRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(product);
        _authServiceMock.Setup(a => a.Authorize(product, ResourceOperation.Update))
            .Returns(true);
        _mapperMock.Setup(m => m.Map(command, product));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _productRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _authServiceMock.Verify(a => a.Authorize(product, ResourceOperation.Update), Times.Once);
        _mapperMock.Verify(m => m.Map(command, product), Times.Once);
        _productRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_ProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new EditProductCommand { Id = 1 };

        _productRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync((Product)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        _productRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _authServiceMock.Verify(a => a.Authorize(It.IsAny<Product>(), ResourceOperation.Update), Times.Never);
        _mapperMock.Verify(m => m.Map(It.IsAny<EditProductCommand>(), It.IsAny<Product>()), Times.Never);
        _productRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Handle_UnauthorizedAccess_ThrowsForbidException()
    {
        // Arrange
        var command = new EditProductCommand { Id = 1 };
        var product = new Product { Id = 1, Name = "Original Product Name" };

        _productRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(product);
        _authServiceMock.Setup(a => a.Authorize(product, ResourceOperation.Update))
            .Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<ForbidException>(() => _handler.Handle(command, CancellationToken.None));
        _productRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _authServiceMock.Verify(a => a.Authorize(product, ResourceOperation.Update), Times.Once);
        _mapperMock.Verify(m => m.Map(It.IsAny<EditProductCommand>(), It.IsAny<Product>()), Times.Never);
        _productRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
    
}
