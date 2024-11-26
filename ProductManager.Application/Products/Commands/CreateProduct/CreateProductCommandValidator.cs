using FluentValidation;

namespace ProductManager.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .Length(1, 100);

        RuleFor(p => p.ProduceDate)
            .Must(NotBeInTheFuture)
            .WithMessage("Produce date cannot be in the future.");

        RuleFor(p => p.ProduceDate)
            .NotEmpty()
            .WithMessage("Produce date cannot be empty");

        RuleFor(p => p.ManufactureEmail)
            .EmailAddress();
        
        RuleFor(p => p.ManufacturePhone)
            .Length(11)
            .Must(BeAllNumeric)
            .Must(StartWithZero)
            .WithMessage("Manufacture phone must be in 0xxxxxxxxxx format");
    }
    
    private bool NotBeInTheFuture(DateOnly date)
    {
        return date <= DateOnly.FromDateTime(DateTime.Now);
    }
    
    private bool StartWithZero(string value)
    {
        return !string.IsNullOrEmpty(value) && value.StartsWith("0");
    }

    private bool BeAllNumeric(string value)
    {
        return !string.IsNullOrEmpty(value) && value.All(c => c >= '0' && c <= '9');
    }
}
