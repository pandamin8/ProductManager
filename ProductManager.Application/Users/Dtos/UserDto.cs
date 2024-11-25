namespace ProductManager.Application.Users.Dtos;

public class UserDto
{
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public List<string> Roles { get; set; } = [];
}
