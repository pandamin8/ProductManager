using FluentValidation.TestHelper;
using JetBrains.Annotations;
using ProductManager.Application.Users.Commands.EditUserDetails;
using Xunit;

namespace ProductManager.Application.Tests.Users.Commands.EditUserDetails;

[TestSubject(typeof(EditUserDetailsCommandValidator))]
public class EditUserDetailsCommandValidatorTest
{

    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new EditUserDetailsCommand()
        {
            FirstName = "John",
            LastName = "Doe"
        };
        
        var validator = new EditUserDetailsCommandValidator();
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ForInvalidCommand_ShouldHaveValidationError()
    {
        // Arrange
        var command = new EditUserDetailsCommand()
        {
            FirstName = "J",
            LastName = "D"
        };
        
        var validator = new EditUserDetailsCommandValidator();
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FirstName);
        result.ShouldHaveValidationErrorFor(c => c.LastName);
    }
}
