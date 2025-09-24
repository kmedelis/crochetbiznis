namespace Crochetbiznis.Application.Products.Dtos;

public record ProductDto
(
    int Id,
    string Name,
    string Slug,
    decimal Price,
    string Description,
    int Stock
);