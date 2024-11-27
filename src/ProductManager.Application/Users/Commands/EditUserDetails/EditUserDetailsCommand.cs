using MediatR;

namespace ProductManager.Application.Users.Commands.EditUserDetails;

public class EditUserDetailsCommand : IRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
