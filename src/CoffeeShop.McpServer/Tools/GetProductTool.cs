using System.ComponentModel;
using System.Text.Json;
using CoffeeShop.Models;
using ModelContextProtocol.Server;

namespace CoffeeShop.McpServer.Tools;

[McpServerToolType]
public class GetProductTool
{
    private static readonly HttpClient _httpClient = new();
    private const string ApiBaseUrl = "https://localhost:7073/api";

    [McpServerTool]
    [Description("Get details of a specific product by its ID")]
    public async Task<string> GetProduct(
        [Description("The ID of the product to retrieve")] int productId)
    {
        try
        {
            var url = $"{ApiBaseUrl}/products/{productId}";
            var response = await _httpClient.GetAsync(url);
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return $"Product with ID {productId} not found";
            }
            
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(content, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            return JsonSerializer.Serialize(product, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
        }
        catch (Exception ex)
        {
            return $"Error retrieving product {productId}: {ex.Message}";
        }
    }
}