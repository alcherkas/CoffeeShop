using System.Text;
using System.Text.Json;
using CoffeeShop.Models;

namespace CoffeeShop.Web.Services;

public class ApiBasketService : IApiBasketService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiBasketService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<IEnumerable<BasketItem>> GetBasketItemsAsync(string sessionId)
    {
        var response = await _httpClient.GetAsync($"/api/basket/{sessionId}");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<BasketItem>>(json, _jsonOptions) ?? [];
    }

    public async Task<bool> AddToBasketAsync(string sessionId, int productId, int quantity = 1)
    {
        var request = new { ProductId = productId, Quantity = quantity };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync($"/api/basket/{sessionId}/items", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateQuantityAsync(string sessionId, int basketItemId, int quantity)
    {
        var request = new { Quantity = quantity };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync($"/api/basket/{sessionId}/items/{basketItemId}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveFromBasketAsync(string sessionId, int basketItemId)
    {
        var response = await _httpClient.DeleteAsync($"/api/basket/{sessionId}/items/{basketItemId}");
        return response.IsSuccessStatusCode;
    }

    public async Task ClearBasketAsync(string sessionId)
    {
        await _httpClient.DeleteAsync($"/api/basket/{sessionId}");
    }

    public async Task<int> GetBasketCountAsync(string sessionId)
    {
        var response = await _httpClient.GetAsync($"/api/basket/{sessionId}/count");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Dictionary<string, int>>(json, _jsonOptions);
        return result?.GetValueOrDefault("count", 0) ?? 0;
    }

    public async Task<BasketSummary> GetBasketSummaryAsync(string sessionId)
    {
        var response = await _httpClient.GetAsync($"/api/basket/{sessionId}/summary");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BasketSummary>(json, _jsonOptions) ?? new BasketSummary();
    }
}