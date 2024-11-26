using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Products.Commands.CreateProduct;
using ProductManager.Application.Products.Dtos;
using ProductManager.Application.Products.Queries;

namespace ProductManager.API.Controllers;

[ApiController]
[Route("/products")]
public class ProductController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductCommand command)
    {
        var id = await mediator.Send(command);

        return CreatedAtAction(nameof(GetProductByIdAsync), new { id }, null);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProductByIdAsync([FromRoute] int id)
    {
        var product = await mediator.Send(new GetProductByIdQuery(id));
        return Ok(product);
    }
    
}
