using MediatR;
using ProductManager.Application.Common;
using ProductManager.Application.Products.Dtos;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Products.Queries.GetMatchingProducts;

public class GetMatchingProductsQuery : IRequest<PageResult<ProductDto>>
{
    public string? SearchPhrase { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
