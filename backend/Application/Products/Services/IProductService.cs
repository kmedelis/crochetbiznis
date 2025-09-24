using Crochetbiznis.Application.Products.Dtos;
using Crochetbiznis.Application.Products.Queries;
using Crochetbiznis.Models;

namespace Crochetbiznis.Application.Products.Services;


public interface IProductService
{
    Task<ProductDto?> GetProductAsync(int id);
    Task<Product> CreateProduct(ProductCreateDto dto, CancellationToken ct);
    Task<List<ProductDto>> ListProductsAsync(ProductQuery query);
    Task DeleteProducAsync(int id);
}