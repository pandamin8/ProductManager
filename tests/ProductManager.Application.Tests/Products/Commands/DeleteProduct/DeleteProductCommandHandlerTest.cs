using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManager.Application.Products.Commands.DeleteProduct;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;
using ProductManager.Domain.Interfaces;
using ProductManager.Domain.Repositories;
using Xunit;

namespace ProductManager.Application.Tests.Products.Commands.DeleteProduct;

[TestSubject(typeof(DeleteProductCommandHandler))]
public class DeleteProductCommandHandlerTest
{
    private readonly Mock<ILogger<DeleteProductCommandHandler>> _loggerMock;
    private readonly Mock<IProductsRepository> _productsRepositoryMock;
    private readonly Mock<IProductAuthorizationService> _authServiceMock;
    private readonly DeleteProductCommandHandler _handler;

    public DeleteProductCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<DeleteProductCommandHandler>>();
        _productsRepositoryMock = new Mock<IProductsRepository>();
        _authServiceMock = new Mock<IProductAuthorizationService>();

        _handler = new DeleteProductCommandHandler(
            _loggerMock.Object,
            _productsRepositoryMock.Object,
            _authServiceMock.Object
        );
    }
    
    [Fact]
    public async Task Handle_ForValidRequest_DeletesProductSuccessfully()
    {
        // Arrange
        var command = new DeleteProductCommand(1);
        var product = new Product { Id = 1, Name = "Test Product" };

        _productsRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(product);
        _authServiceMock.Setup(a => a.Authorize(product, ResourceOperation.Delete))
            .Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _productsRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _authServiceMock.Verify(a => a.Authorize(product, ResourceOperation.Delete), Times.Once);
        _productsRepositoryMock.Verify(r => r.DeleteAsync(product), Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_ProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new DeleteProductCommand(1);

        _productsRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync((Product)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _productsRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _authServiceMock.Verify(a => a.Authorize(It.IsAny<Product>(), ResourceOperation.Delete), Times.Never);
        _productsRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Product>()), Times.Never);
    }
    
    
    [Fact]
    public async Task Handle_UnauthorizedAccess_ThrowsForbidException()
    {
        // Arrange
        var command = new DeleteProductCommand(1);
        var product = new Product { Id = 1, Name = "Test Product" };

        _productsRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(product);
        _authServiceMock.Setup(a => a.Authorize(product, ResourceOperation.Delete))
            .Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<ForbidException>(() => _handler.Handle(command, CancellationToken.None));

        _productsRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _authServiceMock.Verify(a => a.Authorize(product, ResourceOperation.Delete), Times.Once);
        _productsRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Product>()), Times.Never);
    }
}
