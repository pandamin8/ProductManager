using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Products.Commands.CreateProduct;

namespace ProductManager.API.Controllers;

[ApiController]
[Route("/products")]
[Authorize(Roles = "User")]
public class ProductController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductCommand command)
    {
        var id = await mediator.Send(command);

        return Created();
    }
}
