using MediatR;
using Microsoft.Extensions.Logging;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;
using ProductManager.Domain.Interfaces;
using ProductManager.Domain.Repositories;

namespace ProductManager.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(
    ILogger<DeleteProductCommandHandler> logger,
    IProductsRepository productsRepository,
    IProductAuthorizationService authService) : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting product {ProductId}.", request.Id);
        
        var product = await productsRepository.GetByIdAsync(request.Id);
        
        if (product == null)
            throw new NotFoundException(nameof(Product), request.Id);

        if (!authService.Authorize(product, ResourceOperation.Delete))
            throw new ForbidException();
        
        await productsRepository.DeleteAsync(product);
    }
}
