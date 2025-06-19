using System.ComponentModel;
using System.Text.Json;
using CoffeeShop.Models;
using ModelContextProtocol.Server;

namespace CoffeeShop.McpServer.Tools;

[McpServerToolType]
public class GetBasketTool
{
    private static readonly HttpClient _httpClient = new();
    private const string ApiBaseUrl = "https://localhost:7073/api";

    [McpServerTool]
    [Description("Get the current basket contents for a session")]
    public async Task<string> GetBasket(
        [Description("The session ID to retrieve the basket for")] string sessionId)
    {
        try
        {
            var url = $"{ApiBaseUrl}/basket/{Uri.EscapeDataString(sessionId)}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var basketItems = JsonSerializer.Deserialize<BasketItem[]>(content, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            return JsonSerializer.Serialize(basketItems, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
        }
        catch (Exception ex)
        {
            return $"Error retrieving basket for session {sessionId}: {ex.Message}";
        }
    }

    [McpServerTool]
    [Description("Get the basket summary including total items and price for a session")]
    public async Task<string> GetBasketSummary(
        [Description("The session ID to retrieve the basket summary for")] string sessionId)
    {
        try
        {
            var url = $"{ApiBaseUrl}/basket/{Uri.EscapeDataString(sessionId)}/summary";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        catch (Exception ex)
        {
            return $"Error retrieving basket summary for session {sessionId}: {ex.Message}";
        }
    }
}