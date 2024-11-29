using ProductManager.Domain.Constants;

namespace ProductManager.Domain.Types;

public record GetAllMatchingProductsInput(
    string? SearchPhrase,
    int PageSize,
    int PageNumber,
    string? SortBy,
    SortDirection SortDirection,
    string? UserId
    );
