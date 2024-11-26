using System;
using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Moq;
using ProductManager.Application.Users;
using ProductManager.Domain.Constants;
using Xunit;

namespace ProductManager.Application.Tests.Users;

[TestSubject(typeof(UserContext))]
public class UserContextTest
{
    [Fact]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@test.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User)
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        httpContextAccessorMock.SetupGet(c => c.HttpContext).Returns(new DefaultHttpContext()
        {
            User = user
        });

        var userContext = new UserContext(httpContextAccessorMock.Object);


        var currentUser = userContext.GetCurrentUser();
        
        currentUser.Should().NotBeNull();
        currentUser!.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
    }

    [Fact]
    public void GetCurrentUser_WithNullUser_ShouldThrowInvalidOperationException()
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.SetupGet(c => c.HttpContext).Returns((HttpContext)null);
        
        var userContext = new UserContext(httpContextAccessorMock.Object);
        
        Action action = () => userContext.GetCurrentUser();
        
        action.Should().Throw<InvalidOperationException>();
    }
}
