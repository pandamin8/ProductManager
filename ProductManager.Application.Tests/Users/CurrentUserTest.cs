using FluentAssertions;
using JetBrains.Annotations;
using ProductManager.Application.Users;
using ProductManager.Domain.Constants;
using Xunit;

namespace ProductManager.Application.Tests.Users;

[TestSubject(typeof(CurrentUser))]
public class CurrentUserTest
{

    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User]);
        
        var isInRole = currentUser.IsInRole(roleName);
        
        isInRole.Should().BeTrue();
    }
    
    [Fact]
    public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.User]);
        
        var isInRole = currentUser.IsInRole(UserRoles.Admin);
        
        isInRole.Should().BeFalse();
    }
}
