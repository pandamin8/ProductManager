using FluentValidation.TestHelper;
using JetBrains.Annotations;
using ProductManager.Application.Users.Commands.AssignUserRole;
using ProductManager.Application.Users.Commands.UnassignUserRole;
using Xunit;

namespace ProductManager.Application.Tests.Users.Commands.UnassignUserRole;

[TestSubject(typeof(UnassignUserRoleCommandValidator))]
public class UnassignUserRoleCommandValidatorTest
{

    [Fact]
    public void Validator_ForValidCommand_ShouldNotHavaValidationError()
    {
        var command = new AssignUserRoleCommand()
        {
            RoleName = "Admin",
            UserEmail = "test@test.com"
        };
        var validator = new AssignUserRoleCommandValidator();

        var result = validator.TestValidate(command);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validator_ForInvalidCommand_ShouldHavaValidationErrors()
    {
        var command = new AssignUserRoleCommand()
        {
            RoleName = null!,
            UserEmail = "test.com"
        };
        var validator = new AssignUserRoleCommandValidator();

        var result = validator.TestValidate(command);
        
        result.ShouldHaveValidationErrorFor(c => c.RoleName);
        result.ShouldHaveValidationErrorFor(c => c.UserEmail);
    }
}
