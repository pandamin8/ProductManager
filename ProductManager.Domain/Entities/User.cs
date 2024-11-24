using Microsoft.AspNetCore.Identity;

namespace ProductManager.Domain.Entities;

public class User : IdentityUser
{
    public List<Product> Products { get; set; } = [];
}
