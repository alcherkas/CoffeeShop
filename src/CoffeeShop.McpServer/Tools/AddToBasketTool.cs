using System.ComponentModel;
using System.Text;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace CoffeeShop.McpServer.Tools;

[McpServerToolType]
public class AddToBasketTool
{
    private static readonly HttpClient _httpClient = new();
    private const string ApiBaseUrl = "https://localhost:7073/api";

    [McpServerTool]
    [Description("Add a product to the shopping basket")]
    public async Task<string> AddToBasket(
        [Description("The session ID for the basket")] string sessionId,
        [Description("The ID of the product to add")] int productId,
        [Description("The quantity to add (default: 1)")] int quantity = 1)
    {
        try
        {
            var url = $"{ApiBaseUrl}/basket/{Uri.EscapeDataString(sessionId)}/items";
            
            var requestBody = new
            {
                ProductId = productId,
                Quantity = quantity
            };
            
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(url, content);
            
            if (response.IsSuccessStatusCode)
            {
                return $"Successfully added {quantity} item(s) of product {productId} to basket for session {sessionId}";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return $"Failed to add item to basket: {response.StatusCode} - {errorContent}";
            }
        }
        catch (Exception ex)
        {
            return $"Error adding item to basket: {ex.Message}";
        }
    }
}