using MediatR;

namespace ProductManager.Application.Products.Commands.EditProduct;

public class EditProductCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsAvailable { get; set; }
}
