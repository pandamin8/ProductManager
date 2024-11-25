using FluentValidation;

namespace ProductManager.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(user => user.Email)
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(user => user.Password)
            .NotEmpty();

        RuleFor(user => user.FirstName)
            .NotEmpty();

        RuleFor(user => user.LastName)
            .NotEmpty();
    }
}
