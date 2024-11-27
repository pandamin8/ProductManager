using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManager.Application.Users.Commands.UnassignUserRole;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;
using Xunit;

namespace ProductManager.Application.Tests.Users.Commands.UnassignUserRole;

[TestSubject(typeof(UnassignUserRoleCommandHandler))]
public class UnassignUserRoleCommandHandlerTest
{
    private Mock<ILogger<UnassignUserRoleCommandHandler>> _loggerMock;
    private Mock<UserManager<User>> _userManagerMock;
    private Mock<RoleManager<IdentityRole>> _roleManagerMock;


    public UnassignUserRoleCommandHandlerTest()
    {
        // Arrange
        _loggerMock = new Mock<ILogger<UnassignUserRoleCommandHandler>>();
        
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null!, null!, null!, null!, null!, null!, null!, null!);
        
        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            Mock.Of<IRoleStore<IdentityRole>>(),
            null!, null!, null!, null!);
    }

    [Fact]
    public async Task Handle_ForValidRequest_UnassignUserFromRole()
    {
        // Arrange
        var command = new UnassignUserRoleCommand
        {
            UserEmail = "test@example.com",
            RoleName = "Admin"
        };

        var user = new User { Email = command.UserEmail };
        var role = new IdentityRole { Name = command.RoleName };

        _userManagerMock.Setup(um => um.FindByEmailAsync(command.UserEmail))
            .ReturnsAsync(user);
        _roleManagerMock.Setup(rm => rm.FindByNameAsync(command.RoleName))
            .ReturnsAsync(role);
        _userManagerMock.Setup(um => um.RemoveFromRoleAsync(user, command.RoleName))
            .ReturnsAsync(IdentityResult.Success);

        var handler = new UnassignUserRoleCommandHandler(
            _loggerMock.Object, _userManagerMock.Object, _roleManagerMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _userManagerMock.Verify(um => um.FindByEmailAsync(command.UserEmail), Times.Once);
        _roleManagerMock.Verify(rm => rm.FindByNameAsync(command.RoleName), Times.Once);
        _userManagerMock.Verify(um => um.RemoveFromRoleAsync(user, command.RoleName), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new UnassignUserRoleCommand
        {
            UserEmail = "missing@example.com",
            RoleName = "Admin"
        };

        _userManagerMock.Setup(um => um.FindByEmailAsync(command.UserEmail))
            .ReturnsAsync((User)null);

        var handler = new UnassignUserRoleCommandHandler(
            _loggerMock.Object, _userManagerMock.Object, _roleManagerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

        _userManagerMock.Verify(um => um.FindByEmailAsync(command.UserEmail), Times.Once);
        _roleManagerMock.Verify(rm => rm.FindByNameAsync(It.IsAny<string>()), Times.Never);
        _userManagerMock.Verify(um => um.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRoleNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new UnassignUserRoleCommand
        {
            UserEmail = "test@example.com",
            RoleName = "MissingRole"
        };

        var user = new User { Email = command.UserEmail };

        _userManagerMock.Setup(um => um.FindByEmailAsync(command.UserEmail))
            .ReturnsAsync(user);
        _roleManagerMock.Setup(rm => rm.FindByNameAsync(command.RoleName))
            .ReturnsAsync((IdentityRole)null);

        var handler = new UnassignUserRoleCommandHandler(
            _loggerMock.Object, _userManagerMock.Object, _roleManagerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

        _userManagerMock.Verify(um => um.FindByEmailAsync(command.UserEmail), Times.Once);
        _roleManagerMock.Verify(rm => rm.FindByNameAsync(command.RoleName), Times.Once);
        _userManagerMock.Verify(um => um.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
    }
}
