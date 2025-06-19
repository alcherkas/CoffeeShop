using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace CoffeeShop.McpServer.Tools;

[McpServerToolType]
public class GetCategororiesTool
{
    private static readonly HttpClient _httpClient = new();
    private const string ApiBaseUrl = "https://localhost:7073/api";

    [McpServerTool]
    [Description("Get all available product categories in the coffee shop")]
    public async Task<string> GetCategories()
    {
        try
        {
            var url = $"{ApiBaseUrl}/products/categories";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var categories = JsonSerializer.Deserialize<string[]>(content, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            return JsonSerializer.Serialize(categories, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
        }
        catch (Exception ex)
        {
            return $"Error retrieving categories: {ex.Message}";
        }
    }
}