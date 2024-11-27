using FluentValidation;
using ProductManager.Application.Products.Dtos;

namespace ProductManager.Application.Products.Queries.GetMatchingProducts;

public class GetMatchingProductsQueryValidator : AbstractValidator<GetMatchingProductsQuery>
{
    private readonly string[] _allowedSortByColumns = [nameof(ProductDto.Name), nameof(ProductDto.ProduceDate)];
    
    public GetMatchingProductsQueryValidator()
    {
        RuleFor(q => q.PageSize)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.PageSize)
            .LessThanOrEqualTo(20);
        
        RuleFor(q => q.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.SortBy)
            .Must(value => _allowedSortByColumns.Contains(value))
            .When(q => q.SortBy != null)
            .WithMessage("Invalid sort value.");

    }
}
