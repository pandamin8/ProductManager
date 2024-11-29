using ProductManager.Application.Users.Dtos;

namespace ProductManager.Application.Products.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateOnly ProduceDate { get; set; }
    public bool? IsAvailable { get; set; } = true;
    public string ManufacturePhone { get; set; } = default!;
    public string ManufactureEmail { get; set; } = default!;
    public UserDto User { get; set; } = default!;
}
