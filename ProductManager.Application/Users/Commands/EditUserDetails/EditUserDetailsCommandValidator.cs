using FluentValidation;

namespace ProductManager.Application.Users.Commands.EditUserDetails;

public class EditUserDetailsCommandValidator : AbstractValidator<EditUserDetailsCommand>
{
    public EditUserDetailsCommandValidator()
    {
        RuleFor(user => user.FirstName)
            .Length(3, 100);
        
        RuleFor(user => user.LastName)
            .Length(3, 100);
    }
}
