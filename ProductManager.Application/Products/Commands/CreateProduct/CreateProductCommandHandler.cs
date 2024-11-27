using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManager.Application.Users;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;
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

        var duplicateEmail = await productsRepository.GetByManufacturerEmail(request.ManufactureEmail);

        if (duplicateEmail != null)
            throw new ConflictException("Product with manufacturer email already exists");
        
        var duplicatePhone = await productsRepository.GetByManufacturerPhoneNumber(request.ManufacturePhone);
        
        if (duplicatePhone != null)
            throw new ConflictException("Product with manufacturer phone already exists");
        
        var product = mapper.Map<Product>(request);
        product.UserId = currentUser.Id;
        var id = await productsRepository.CreateAsync(product);
        
        return id;
    }
}
