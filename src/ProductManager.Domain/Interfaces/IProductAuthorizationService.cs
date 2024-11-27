using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;

namespace ProductManager.Domain.Interfaces;

public interface IProductAuthorizationService
{
    bool Authorize(Product product, ResourceOperation resourceOperation);
}
