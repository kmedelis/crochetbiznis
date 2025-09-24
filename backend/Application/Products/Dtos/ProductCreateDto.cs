using System.ComponentModel.DataAnnotations;

namespace Crochetbiznis.Application.Products.Dtos;

public record ProductCreateDto(
    [property: Required, StringLength(80, MinimumLength = 1)] string Name,
    [property: Range(0.01, 10000)] decimal Price,
    [property: Range(1, 10000)] int Stock,
    string Description
);