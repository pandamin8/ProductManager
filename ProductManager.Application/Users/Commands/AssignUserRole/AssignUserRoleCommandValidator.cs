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
            .Must(RoleExists)
            .WithMessage("Role doesn't exist.");
    }
    
    private bool RoleExists(string roleName)
    {
        return roleName == UserRoles.Admin || roleName == UserRoles.User;
    }
}
