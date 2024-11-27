namespace ProductManager.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateOnly ProduceDate { get; set; } = default!;
    public bool IsAvailable { get; set; } = true;
    
    public ManufactureContact ManufactureContact { get; set; } = default!;
    
    public User User { get; set; } = default!;
    public string UserId { get; set; } = default!;
}
