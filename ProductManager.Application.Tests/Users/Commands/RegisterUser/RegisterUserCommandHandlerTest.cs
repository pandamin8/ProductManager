using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManager.Application.Users.Commands.RegisterUser;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;
using Xunit;

namespace ProductManager.Application.Tests.Users.Commands.RegisterUser;

public class RegisterUserCommandHandlerTest
{
    private readonly Mock<ILogger<RegisterUserCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly RegisterUserCommand _command;


    public RegisterUserCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<RegisterUserCommandHandler>>();
        _mapperMock = new Mock<IMapper>();

        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null!, null!, null!, null!, null!, null!, null!, null!);

        _command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = "Password123!"
        };
    }


    [Fact]
    public async Task Handle_ForValidCommand_DoesNotThrowException()
    {
        // Arrange
        var user = new User { Email = _command.Email };

        _mapperMock.Setup(m => m.Map<User>(_command)).Returns(user);

        _userManagerMock.Setup(um => um.FindByEmailAsync(_command.Email))
            .ReturnsAsync((User)null); // Simulate no existing user

        _userManagerMock.Setup(um => um.CreateAsync(user, _command.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(um => um.AddToRoleAsync(user, UserRoles.User))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(um => um.DeleteAsync(user));

        var handler = new RegisterUserCommandHandler(
            _loggerMock.Object, _userManagerMock.Object, _mapperMock.Object);

        // Act
        await handler.Handle(_command, CancellationToken.None);

        // Assert
        _userManagerMock.Verify(um => um.FindByEmailAsync(_command.Email), Times.Once);
        _userManagerMock.Verify(um => um.CreateAsync(user, _command.Password), Times.Once);
        _userManagerMock.Verify(um => um.AddToRoleAsync(user, UserRoles.User), Times.Once);
        _userManagerMock.Verify(um => um.DeleteAsync(user), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserAlreadyExists_ThrowsConflictException()
    {
        // Arrange
        var existingUser = new User { Email = _command.Email };

        _userManagerMock.Setup(um => um.FindByEmailAsync(_command.Email))
            .ReturnsAsync(existingUser);

        var handler = new RegisterUserCommandHandler(
            _loggerMock.Object, _userManagerMock.Object, _mapperMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => handler.Handle(_command, CancellationToken.None));
        _userManagerMock.Verify(um => um.FindByEmailAsync(_command.Email), Times.Once);
        _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRoleAssignmentFails_ThrowsException()
    {
        // Arrange
        var user = new User { Email = _command.Email };

        _mapperMock.Setup(m => m.Map<User>(_command)).Returns(user);

        _userManagerMock.Setup(um => um.FindByEmailAsync(_command.Email))
            .ReturnsAsync((User)null);

        _userManagerMock.Setup(um => um.CreateAsync(user, _command.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(um => um.AddToRoleAsync(user, UserRoles.User))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Role assignment failed." }));

        _userManagerMock.Setup(um => um.DeleteAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var handler = new RegisterUserCommandHandler(
            _loggerMock.Object, _userManagerMock.Object, _mapperMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(_command, CancellationToken.None));
        Assert.Contains("failed to assign role", exception.Message);

        _userManagerMock.Verify(um => um.FindByEmailAsync(_command.Email), Times.Once);
        _userManagerMock.Verify(um => um.CreateAsync(user, _command.Password), Times.Once);
        _userManagerMock.Verify(um => um.AddToRoleAsync(user, UserRoles.User), Times.Once);
        _userManagerMock.Verify(um => um.DeleteAsync(user), Times.Once);
    }
}
