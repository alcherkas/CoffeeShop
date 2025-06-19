using CoffeeShop.Models;

namespace CoffeeShop.Web.Services;

public interface IApiProductService
{
    Task<IEnumerable<Product>> GetAvailableProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    Task<IEnumerable<string>> GetCategoriesAsync();
}