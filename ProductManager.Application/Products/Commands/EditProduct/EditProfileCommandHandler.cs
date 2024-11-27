using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;
using ProductManager.Domain.Interfaces;
using ProductManager.Domain.Repositories;

namespace ProductManager.Application.Products.Commands.EditProduct;

public class EditProfileCommandHandler(
    ILogger<EditProfileCommandHandler> logger,
    IProductsRepository productRepository,
    IMapper mapper,
    IProductAuthorizationService authService) : IRequestHandler<EditProductCommand>
{
    public async Task Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating product {ProductId} with {Request}", request.Id, request);
        
        var product = await productRepository.GetByIdAsync(request.Id);

        if (product == null)
            throw new NotFoundException(nameof(Product), request.Id);

        if (!authService.Authorize(product, ResourceOperation.Update))
            throw new ForbidException();
        
        mapper.Map(request, product);
        
        await productRepository.SaveChangesAsync();
    }
}
