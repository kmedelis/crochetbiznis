using System.Threading.Tasks;
using Crochetbiznis.Application.Products.Dtos;
using Crochetbiznis.Application.Products.Queries;
using Crochetbiznis.Application.Products.Services;
using Crochetbiznis.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{id:int}")]
    public ActionResult<List<Product>> GetProducts(int id)
    {
        var dto = _productService.GetProductAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> ListProducts([FromQuery] ProductQuery q)
    {
        var normalizedQuery = q.Normalize();
        var dtos = await _productService.ListProductsAsync(normalizedQuery);

        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(ProductCreateDto dto, CancellationToken ct)
    {
        var created = await _productService.CreateProduct(dto, ct);
        return CreatedAtAction(nameof(CreateProduct), new
        {
            id = created.Id
        }, created);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProducAsync(id);
        return NoContent();
    }
}