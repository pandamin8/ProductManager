using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManager.Application.Products.Dtos;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;
using ProductManager.Domain.Repositories;

namespace ProductManager.Application.Products.Queries;

public class GetProductByIdQueryHandler(
    ILogger<GetProductByIdQueryHandler> logger,
    IProductsRepository productsRepository,
    IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving product with id {ProductId}", request.Id);
        
        var restaurant = await productsRepository.GetByIdAsync(request.Id)
                         ?? throw new NotFoundException(nameof(Product), request.Id);

        var productDto = mapper.Map<ProductDto>(restaurant);
        return productDto;
    }
}
