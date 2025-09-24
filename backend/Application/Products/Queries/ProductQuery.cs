namespace Crochetbiznis.Application.Products.Queries;

public record ProductQuery(string? Search, int Page = 1, int PageSize = 20)
{
    public ProductQuery Normalize() =>
        this with { Page = Math.Max(1, Page), PageSize = Math.Clamp(PageSize, 1, 100) };
}
