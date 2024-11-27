using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Common;
using ProductManager.Application.Products.Commands.CreateProduct;
using ProductManager.Application.Products.Commands.EditProduct;
using ProductManager.Application.Products.Dtos;
using ProductManager.Application.Products.Queries;
using ProductManager.Application.Products.Queries.GetMatchingProducts;

namespace ProductManager.API.Controllers;

[ApiController]
[Route("/products")]
public class ProductController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var id = await mediator.Send(command);

        return CreatedAtAction(nameof(GetProductById), new { id }, null);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProductById([FromRoute] int id)
    {
        var product = await mediator.Send(new GetProductByIdQuery(id));
        return Ok(product);
    }

    [HttpPatch("{id:int}")]
    [Authorize]
    public async Task<IActionResult> EditProduct([FromRoute] int id, [FromBody] EditProductCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return AcceptedAtAction(nameof(GetProductById), new { id }, null);
    }

    [HttpGet]
    public async Task<ActionResult<PageResult<ProductDto>>> GetMatchingProducts(
        [FromQuery] GetMatchingProductsQuery query)
    {
        var products = await mediator.Send(query);
        return Ok(products);
    }
}
