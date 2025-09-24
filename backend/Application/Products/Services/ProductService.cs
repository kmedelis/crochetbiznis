using Crochetbiznis.Application.Common.Exceptions;
using Crochetbiznis.Application.Products.Dtos;
using Crochetbiznis.Application.Products.Queries;
using Crochetbiznis.Application.Products.Services;
using Crochetbiznis.Infrastructure.Persistence;
using Crochetbiznis.Models;
using Microsoft.EntityFrameworkCore;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateProduct(ProductCreateDto dto, CancellationToken ct)
    {
        var name = dto.Name.Trim();
        var slugified = Slugify(name);

        var alreadyExists = await _context.Products.AnyAsync(p => p.Slug == slugified);

        if (alreadyExists)
            throw new DuplicateSlugException(slugified);

        var entity = new Product
        {
            Name = name,
            Slug = Slugify(name),
            Description = dto.Description.Trim(),
            Price = dto.Price,
            Stock = dto.Stock
        };
        _context.Products.Add(entity);

        try { await _context.SaveChangesAsync(ct); }
        catch (DbUpdateException ex)
        { throw new DuplicateSlugException(slugified, ex); }


        return entity;
    }


    public async Task<ProductDto?> GetProductAsync(int id) =>
        await _context.Products
            .Where(p => p.Id == id)
            .AsNoTracking()
            .Select(p => new ProductDto(p.Id, p.Name, p.Slug, p.Price, p.Description, p.Stock))
            .FirstOrDefaultAsync();

    public async Task<List<ProductDto>> ListProductsAsync(ProductQuery q) =>
        await _context.Products
            .AsNoTracking()
            .Where(p =>
                string.IsNullOrWhiteSpace(q.Search) ||
                p.Name.ToLower().Contains(q.Search!.Trim().ToLower()) ||
                (p.Description != null && p.Description.ToLower().Contains(q.Search!.Trim().ToLower()))
            )
            .OrderBy(p => p.Name)
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize)
            .Select(p => new ProductDto(p.Id, p.Name, p.Slug, p.Price, p.Description, p.Stock))
            .ToListAsync();

    public async Task DeleteProducAsync(int id)
    {
        var entity = await _context.Products.FindAsync(id);
        if (entity == null)
        {
            throw new ProductNotFoundException(id);
        }
        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static string Slugify(string s)
    {
        var trimmed = string.Join(' ', s.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        return trimmed.ToLowerInvariant().Replace(' ', '-');
    }

}