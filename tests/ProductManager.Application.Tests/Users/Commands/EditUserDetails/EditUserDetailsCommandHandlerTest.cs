using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManager.Application.Users;
using ProductManager.Application.Users.Commands.EditUserDetails;
using ProductManager.Application.Users.Queries.GetCurrentUser;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;
using Xunit;

namespace ProductManager.Application.Tests.Users.Commands.EditUserDetails;

[TestSubject(typeof(EditUserDetailsCommandHandler))]
public class EditUserDetailsCommandHandlerTest
{
    private Mock<ILogger<EditUserDetailsCommandHandler>> _loggerMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private EditUserDetailsCommand _command;

    public EditUserDetailsCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<EditUserDetailsCommandHandler>>();

        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null!, null!, null!, null!, null!, null!, null!, null!);

        _userContextMock = new Mock<IUserContext>();
        
        _command = new EditUserDetailsCommand()
        {
            FirstName = "John",
            LastName = "Doe"
        };
    }

    [Fact]
    public async Task Handle_ForValidCommand_EditUserDetails()
    {
        // Arrange

        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.User]);

        _userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var user = new User();
        
        _userManagerMock.Setup(um => um.FindByIdAsync(currentUser.Id)).ReturnsAsync(user);
        
        var commandHandler =
            new EditUserDetailsCommandHandler(_loggerMock.Object, _userManagerMock.Object, _userContextMock.Object);

        // Act
        await commandHandler.Handle(_command, CancellationToken.None);
        
        // Assert
        user.FirstName.Should().Be("John");
        user.LastName.Should().Be("Doe");
        _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ForNoUserFound_ThrowNotFoundException()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.User]);

        _userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);
        
        _userManagerMock.Setup(um => um.FindByIdAsync(currentUser.Id)).ReturnsAsync((User)null);
        
        var commandHandler =
            new EditUserDetailsCommandHandler(_loggerMock.Object, _userManagerMock.Object, _userContextMock.Object);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => commandHandler.Handle(_command, CancellationToken.None));
        exception.Message.Should().Contain("not found");
    }
}
