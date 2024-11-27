using FluentValidation;

namespace ProductManager.Application.Products.Commands.EditProduct;

public class EditProductCommandValidator : AbstractValidator<EditProductCommand>
{
    public EditProductCommandValidator()
    {
        RuleFor(c => c.Name)
            .Length(1, 100);
    }
}
