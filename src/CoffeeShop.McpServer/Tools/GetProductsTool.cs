using System.ComponentModel;
using System.Text.Json;
using CoffeeShop.Models;
using ModelContextProtocol.Server;

namespace CoffeeShop.McpServer.Tools;

[McpServerToolType]
public class GetProductsTool
{
    private static readonly HttpClient _httpClient = new();
    private const string ApiBaseUrl = "https://localhost:7073/api";

    [McpServerTool]
    [Description("Get all available coffee shop products, optionally filtered by category or search term")]
    public async Task<string> GetProducts(
        [Description("Optional category to filter products by")] string? category = null,
        [Description("Optional search term to find products")] string? search = null)
    {
        try
        {
            var url = $"{ApiBaseUrl}/products";
            var queryParams = new List<string>();
            
            if (!string.IsNullOrEmpty(category))
                queryParams.Add($"category={Uri.EscapeDataString(category)}");
            
            if (!string.IsNullOrEmpty(search))
                queryParams.Add($"search={Uri.EscapeDataString(search)}");
            
            if (queryParams.Any())
                url += "?" + string.Join("&", queryParams);

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<Product[]>(content, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            return JsonSerializer.Serialize(products, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
        }
        catch (Exception ex)
        {
            return $"Error retrieving products: {ex.Message}";
        }
    }
}