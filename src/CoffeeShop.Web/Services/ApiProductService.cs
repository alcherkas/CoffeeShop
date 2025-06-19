using System.Text.Json;
using CoffeeShop.Models;

namespace CoffeeShop.Web.Services;

public class ApiProductService : IApiProductService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<IEnumerable<Product>> GetAvailableProductsAsync()
    {
        var response = await _httpClient.GetAsync("/api/products");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Product>>(json, _jsonOptions) ?? [];
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/products/{id}");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Product>(json, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
    {
        var response = await _httpClient.GetAsync($"/api/products?category={Uri.EscapeDataString(category)}");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Product>>(json, _jsonOptions) ?? [];
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        var response = await _httpClient.GetAsync($"/api/products?search={Uri.EscapeDataString(searchTerm)}");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Product>>(json, _jsonOptions) ?? [];
    }

    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("/api/products/categories");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<string>>(json, _jsonOptions) ?? [];
    }
}