using MediatR;
using ProductManager.Application.Products.Dtos;

namespace ProductManager.Application.Products.Queries;

public class GetProductByIdQuery(int id) : IRequest<ProductDto>
{
    public int Id { get; set; } = id;
}
