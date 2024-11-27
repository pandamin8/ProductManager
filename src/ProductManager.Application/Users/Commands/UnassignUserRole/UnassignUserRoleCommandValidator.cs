using FluentValidation;
using ProductManager.Domain.Constants;

namespace ProductManager.Application.Users.Commands.UnassignUserRole;

public class UnassignUserRoleCommandValidator : AbstractValidator<UnassignUserRoleCommand>
{
    public UnassignUserRoleCommandValidator()
    {
        RuleFor(user => user.UserEmail)
            .NotEmpty()
            .EmailAddress();
        
        RuleFor(user => user.RoleName)
            .NotEmpty()
            .WithMessage("Role doesn't exist.");
    }
}
