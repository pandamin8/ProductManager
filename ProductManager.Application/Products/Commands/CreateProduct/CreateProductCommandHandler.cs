using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManager.Application.Users;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Repositories;

namespace ProductManager.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(
    ILogger<CreateProductCommandHandler> logger,
    IMapper mapper,
    IProductsRepository productsRepository,
    IUserContext userContext) : IRequestHandler<CreateProductCommand, int>
{
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        
        logger.LogInformation("{UserName} [{UserId}] is creating a new product {@Product}", currentUser!.Email,
            currentUser.Id, request);
        
        var product = mapper.Map<Product>(request);
        product.UserId = currentUser.Id;
        var id = await productsRepository.CreateAsync(product);
        
        return id;
    }
}
