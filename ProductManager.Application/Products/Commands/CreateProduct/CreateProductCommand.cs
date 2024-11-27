using MediatR;

namespace ProductManager.Application.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<int>
{
    public string Name { get; set; } = default!;
    public DateOnly ProduceDate { get; set; }
    public bool? IsAvailable { get; set; } = true;
    public string ManufacturePhone { get; set; } = default!;
    public string ManufactureEmail { get; set; } = default!;
}
