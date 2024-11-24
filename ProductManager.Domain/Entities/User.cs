using Microsoft.AspNetCore.Identity;

namespace ProductManager.Domain.Entities;

public class User : IdentityUser
{
    public List<Product> Products { get; set; } = [];
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
