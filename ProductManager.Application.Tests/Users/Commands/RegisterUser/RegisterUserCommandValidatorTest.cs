using FluentValidation.TestHelper;
using JetBrains.Annotations;
using ProductManager.Application.Users.Commands.RegisterUser;
using Xunit;

namespace ProductManager.Application.Tests.Users.Commands.RegisterUser;

[TestSubject(typeof(RegisterUserCommandValidator))]
public class RegisterUserCommandValidatorTest
{

    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationError()
    {
        var command = new RegisterUserCommand()
        {
            Email = "test@test.com",
            Password = "test",
            FirstName = "test",
            LastName = "test"
        };
        var validator = new RegisterUserCommandValidator();
        
        var result = validator.TestValidate(command);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validator_ForInvalidCommand_ShouldHavaValidationError()
    {
        var command = new RegisterUserCommand()
        {
            Email = "test.com",
            Password = "test",
            FirstName = "te",
            LastName = "te"
        };
        var validator = new RegisterUserCommandValidator();
        
        
        var result = validator.TestValidate(command);
        
        result.ShouldHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.FirstName);
        result.ShouldHaveValidationErrorFor(c => c.LastName);
    }
}
