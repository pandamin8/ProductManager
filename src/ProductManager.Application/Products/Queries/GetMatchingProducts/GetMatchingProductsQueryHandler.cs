using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManager.Application.Common;
using ProductManager.Application.Products.Dtos;
using ProductManager.Domain.Repositories;
using ProductManager.Domain.Types;

namespace ProductManager.Application.Products.Queries.GetMatchingProducts;

public class GetMatchingProductsQueryHandler(
    ILogger<GetMatchingProductsQueryHandler> logger,
    IProductsRepository productsRepository,
    IMapper mapper) : IRequestHandler<GetMatchingProductsQuery, PageResult<ProductDto>>
{
    public async Task<PageResult<ProductDto>> Handle(GetMatchingProductsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving matching products.");

        var input = new GetAllMatchingProductsInput(
            SearchPhrase: request.SearchPhrase,
            PageNumber: request.PageNumber,
            PageSize: request.PageSize,
            SortBy: request.SortBy,
            SortDirection: request.SortDirection
        );
        
        var (products, totalCount) =
            await productsRepository.GetAllMatchingAsync(input);

        var productsDto = mapper.Map<IEnumerable<ProductDto>>(products);

        var result = new PageResult<ProductDto>(productsDto, totalCount, request.PageSize, request.PageNumber);

        return result;
    }
}
