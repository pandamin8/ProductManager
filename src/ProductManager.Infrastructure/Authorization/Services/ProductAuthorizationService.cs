using Microsoft.Extensions.Logging;
using ProductManager.Application.Users;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Infrastructure.Authorization.Services;

public class ProductAuthorizationService(ILogger<ProductAuthorizationService> logger, IUserContext userContext) : IProductAuthorizationService
{
    public bool Authorize(Product product, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();
        
        if (user == null)
            throw new UnauthorizedAccessException();

        logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for product {ProductName}",
            user.Email, resourceOperation, product.Name);

        if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
        {
            logger.LogInformation("{Operation} operation - Successful authorization", resourceOperation);
            return true;
        }

        if (resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin user, delete operation - Successful authorization");
            return true;
        }

        if ((resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Delete) &&
            user.IsInRole(UserRoles.User) && user.Id == product.UserId)
        {
            logger.LogInformation("User, {Operation} operation - Successful authorization", resourceOperation);
            return true;
        }

        return false;
    }
}
