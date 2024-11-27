using FluentValidation.TestHelper;
using JetBrains.Annotations;
using ProductManager.Application.Products.Dtos;
using ProductManager.Application.Products.Queries.GetMatchingProducts;
using ProductManager.Domain.Constants;
using Xunit;

namespace ProductManager.Application.Tests.Products.Queries.GetMatchingProducts;

[TestSubject(typeof(GetMatchingProductsQueryValidator))]
public class GetMatchingProductsQueryValidatorTest
{

    [Theory]
    [InlineData(nameof(ProductDto.Name))]
    [InlineData(nameof(ProductDto.ProduceDate))]
    [InlineData(null)]
    public void Validator_ForValidQuery_ShouldNotThrowAnyValidationError(string sortBy)
    {
        // Arrange
        var query = new GetMatchingProductsQuery
        {
            PageSize = 10,
            PageNumber = 2,
            SearchPhrase = "Search Phrase",
            SortBy = sortBy,
            SortDirection = SortDirection.Ascending
        };
        
        var validator = new GetMatchingProductsQueryValidator();
        
        // Act
        var result = validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validator_ForEmptySearchPhrase_ShouldNotThrowAnyValidationError()
    {
        // Arrange
        var query = new GetMatchingProductsQuery
        {
            PageSize = 10,
            PageNumber = 2,
            SearchPhrase = null,
            SortBy = null,
            SortDirection = SortDirection.Ascending
        };
        
        var validator = new GetMatchingProductsQueryValidator();
        
        // Act
        var result = validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validator_ForInvalidQuery_ShouldThrowValidationErrors()
    {
        // Arrange
        var query = new GetMatchingProductsQuery
        {
            PageSize = 100,
            PageNumber = -1,
            SearchPhrase = null,
            SortBy = nameof(ProductDto.ManufactureEmail),
            SortDirection = SortDirection.Ascending
        };
        
        var validator = new GetMatchingProductsQueryValidator();
        
        // Act
        var result = validator.TestValidate(query);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PageSize);
        result.ShouldHaveValidationErrorFor(q => q.SortBy);
        result.ShouldHaveValidationErrorFor(q => q.PageNumber);
    }
}
