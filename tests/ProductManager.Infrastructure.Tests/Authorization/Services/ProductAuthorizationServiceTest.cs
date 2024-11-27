using System;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManager.Application.Users;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Infrastructure.Authorization.Services;
using Xunit;

namespace ProductManager.Infrastructure.Tests.Authorization.Services;

[TestSubject(typeof(ProductAuthorizationService))]
public class ProductAuthorizationServiceTest
{
    private readonly Mock<ILogger<ProductAuthorizationService>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly ProductAuthorizationService _authorizationService;

    public ProductAuthorizationServiceTest()
    {
        _loggerMock = new Mock<ILogger<ProductAuthorizationService>>();
        _userContextMock = new Mock<IUserContext>();
        _authorizationService = new ProductAuthorizationService(_loggerMock.Object, _userContextMock.Object);
    }

    [Fact]
    public void Authorize_WhenUserIsNull_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        _userContextMock.Setup(uc => uc.GetCurrentUser()).Returns((CurrentUser)null);
        var product = new Product { Name = "Test Product", UserId = "123" };

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() =>
            _authorizationService.Authorize(product, ResourceOperation.Read));
    }

    [Fact]
    public void Authorize_WhenOperationIsRead_ReturnsTrue()
    {
        // Arrange
        var user = new CurrentUser("1", "user@test.com", [UserRoles.User]);
        _userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(user);
        var product = new Product { Name = "Test Product", UserId = "123" };

        // Act
        var result = _authorizationService.Authorize(product, ResourceOperation.Read);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Authorize_WhenOperationIsCreate_ReturnsTrue()
    {
        // Arrange
        var user = new CurrentUser("1", "user@test.com", [UserRoles.User]);
        _userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(user);
        var product = new Product { Name = "Test Product", UserId = "123" };

        // Act
        var result = _authorizationService.Authorize(product, ResourceOperation.Create);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Authorize_WhenOperationIsDeleteAndUserIsAdmin_ReturnsTrue()
    {
        // Arrange
        var user = new CurrentUser("1", "admin@test.com", [UserRoles.Admin]);
        _userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(user);
        var product = new Product { Name = "Test Product", UserId = "123" };

        // Act
        var result = _authorizationService.Authorize(product, ResourceOperation.Delete);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Authorize_WhenOperationIsDeleteAndUserIsOwner_ReturnsTrue()
    {
        // Arrange
        var user = new CurrentUser("123", "user@test.com", [UserRoles.User]);
        _userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(user);
        var product = new Product { Name = "Test Product", UserId = "123" };

        // Act
        var result = _authorizationService.Authorize(product, ResourceOperation.Delete);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Authorize_WhenOperationIsUpdateAndUserIsNotOwner_ReturnsFalse()
    {
        // Arrange
        var user = new CurrentUser("456", "user@test.com", [UserRoles.User]);
        _userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(user);
        var product = new Product { Name = "Test Product", UserId = "123" };

        // Act
        var result = _authorizationService.Authorize(product, ResourceOperation.Update);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Authorize_WhenOperationIsDeleteAndUserIsNotAdminOrOwner_ReturnsFalse()
    {
        // Arrange
        var user = new CurrentUser("456", "user@test.com", [UserRoles.User]);
        _userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(user);
        var product = new Product { Name = "Test Product", UserId = "123" };

        // Act
        var result = _authorizationService.Authorize(product, ResourceOperation.Delete);

        // Assert
        result.Should().BeFalse();
    }
}
