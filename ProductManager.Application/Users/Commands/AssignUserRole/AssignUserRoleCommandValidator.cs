using FluentValidation;
using ProductManager.Domain.Constants;

namespace ProductManager.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandValidator : AbstractValidator<AssignUserRoleCommand>
{
    public AssignUserRoleCommandValidator()
    {
        RuleFor(user => user.UserEmail)
            .NotEmpty()
            .EmailAddress();
        
        RuleFor(user => user.RoleName)
            .NotEmpty()
            .WithMessage("Role doesn't exist.");
    }
}
