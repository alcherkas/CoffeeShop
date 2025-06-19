using Microsoft.EntityFrameworkCore;
using CoffeeShop.Core.Data;
using CoffeeShop.Models;

namespace CoffeeShop.Core.Services;

public class ProductService : IProductService
{
    private readonly CoffeeShopContext _context;

    public ProductService(CoffeeShopContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAvailableProductsAsync()
    {
        return await _context.Products
            .Where(p => p.IsAvailable)
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
    {
        return await _context.Products
            .Where(p => p.IsAvailable && p.Category == category)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await _context.Products
            .Where(p => p.IsAvailable && 
                       (p.Name.ToLower().Contains(term) || 
                        p.Description.ToLower().Contains(term) ||
                        p.Category.ToLower().Contains(term)))
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        return await _context.Products
            .Where(p => p.IsAvailable)
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }
}