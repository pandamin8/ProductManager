using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManager.Application.Common;
using ProductManager.Application.Products.Dtos;
using ProductManager.Domain.Repositories;

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

        var (products, totalCount) =
            await productsRepository.GetAllMatchingAsync(
                request.SearchPhrase,
                request.PageSize,
                request.PageNumber,
                request.SortBy,
                request.SortDirection
            );

        var productsDto = mapper.Map<IEnumerable<ProductDto>>(products);

        var result = new PageResult<ProductDto>(productsDto, totalCount, request.PageSize, request.PageNumber);

        return result;
    }
}
