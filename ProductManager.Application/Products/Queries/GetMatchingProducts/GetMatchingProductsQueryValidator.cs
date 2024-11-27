using FluentValidation;

namespace ProductManager.Application.Products.Queries.GetMatchingProducts;

public class GetMatchingProductsQueryValidator : AbstractValidator<GetMatchingProductsQuery>
{
    public GetMatchingProductsQueryValidator()
    {
        RuleFor(q => q.PageSize)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.PageSize)
            .LessThanOrEqualTo(20);
        
        RuleFor(q => q.PageNumber)
            .GreaterThanOrEqualTo(1);
    }
}
