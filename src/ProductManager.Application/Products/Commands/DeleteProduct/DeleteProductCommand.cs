using MediatR;

namespace ProductManager.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommand(int id) : IRequest
{
    public int Id { get; set; } = id;
}
