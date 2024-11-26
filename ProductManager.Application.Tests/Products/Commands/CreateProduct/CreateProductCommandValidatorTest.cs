using System;
using FluentAssertions;
using FluentValidation.TestHelper;
using JetBrains.Annotations;
using ProductManager.Application.Products.Commands.CreateProduct;
using Xunit;

namespace ProductManager.Application.Tests.Products.Commands.CreateProduct;

[TestSubject(typeof(CreateProductCommandValidator))]
public class CreateProductCommandValidatorTest
{
    private CreateProductCommandValidator _validator;

    public CreateProductCommandValidatorTest()
    {
        _validator = new CreateProductCommandValidator();
    }

    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommand()
        {
            Name = "Name",
            ManufactureEmail = "test@test.com",
            ManufacturePhone = "09111111111",
            ProduceDate = DateOnly.FromDateTime(DateTime.Now),
            IsAvailable = true
        };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("11111111111")]
    [InlineData("0bcd1111111")]
    [InlineData("123")]
    public void Validator_ForInvalidCommand_ShouldHaveValidationError(string phoneNumber)
    {
        // Arrange
        var command = new CreateProductCommand()
        {
            Name = "",
            ManufactureEmail = "test.com",
            ManufacturePhone = phoneNumber,
            ProduceDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1),
            IsAvailable = true
        };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Name);
        result.ShouldHaveValidationErrorFor(p => p.ManufactureEmail);
        result.ShouldHaveValidationErrorFor(p => p.ManufacturePhone);
        result.ShouldHaveValidationErrorFor(p => p.ProduceDate);
    }
}
