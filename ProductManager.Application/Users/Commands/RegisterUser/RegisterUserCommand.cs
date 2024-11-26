using MediatR;

namespace ProductManager.Application.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest
{    
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
