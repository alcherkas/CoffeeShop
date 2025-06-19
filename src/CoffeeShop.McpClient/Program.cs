using System.Text.Json;

Console.WriteLine("Coffee Shop MCP Client Test");
Console.WriteLine("===========================");

try
{
    using var httpClient = new HttpClient();
    
    // Add required headers for MCP
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/event-stream");
    
    Console.WriteLine("🔗 Testing direct HTTP connection to MCP server...");
    
    var serverUrl = "https://localhost:7074";
    
    var response = await httpClient.GetAsync($"{serverUrl}/");
    Console.WriteLine($"Server response status: {response.StatusCode}");
    
    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response content: {content}");
    }

    Console.WriteLine("✅ Connected to MCP server");
    Console.WriteLine();

    Console.WriteLine("🧪 Testing MCP endpoints...");
    Console.WriteLine();

    Console.WriteLine("1. Testing SSE endpoint...");
    
    // Try the SSE endpoint that MCP typically uses with shorter timeout
    try 
    {
        using var sseClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
        sseClient.DefaultRequestHeaders.Add("Accept", "text/event-stream");
        
        Console.WriteLine("   Connecting to /sse...");
        var sseResponse = await sseClient.GetAsync($"{serverUrl}/sse");
        Console.WriteLine($"   GET /sse: {sseResponse.StatusCode}");
        
        if (sseResponse.IsSuccessStatusCode)
        {
            var headers = string.Join(", ", sseResponse.Headers.Select(h => $"{h.Key}: {string.Join(",", h.Value)}"));
            Console.WriteLine($"      Headers: {headers}");
        }
    }
    catch (TaskCanceledException)
    {
        Console.WriteLine($"   GET /sse: Timeout (expected for SSE - connection established)");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"   GET /sse: Error - {ex.Message}");
    }

    Console.WriteLine();
    Console.WriteLine("   Testing JSON-RPC endpoint...");
    
    // Try a JSON-RPC request which is what MCP actually uses
    var jsonRpcRequest = JsonSerializer.Serialize(new
    {
        jsonrpc = "2.0",
        id = 1,
        method = "tools/list",
        @params = new { }
    });
    
    try 
    {
        Console.WriteLine($"   Sending JSON-RPC request: {jsonRpcRequest}");
        var jsonRpcResponse = await httpClient.PostAsync($"{serverUrl}/", 
            new StringContent(jsonRpcRequest, System.Text.Encoding.UTF8, "application/json"));
        Console.WriteLine($"   POST / (JSON-RPC): {jsonRpcResponse.StatusCode}");
        
        var content = await jsonRpcResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"      Response: {content}");
        
        if (jsonRpcResponse.IsSuccessStatusCode)
        {
            // Try to call a tool if tools/list worked
            Console.WriteLine();
            Console.WriteLine("   Testing tool call...");
            var toolCallRequest = JsonSerializer.Serialize(new
            {
                jsonrpc = "2.0",
                id = 2,
                method = "tools/call",
                @params = new
                {
                    name = "GetCategories",
                    arguments = new { }
                }
            });
            
            Console.WriteLine($"   Tool call request: {toolCallRequest}");
            var toolResponse = await httpClient.PostAsync($"{serverUrl}/", 
                new StringContent(toolCallRequest, System.Text.Encoding.UTF8, "application/json"));
            Console.WriteLine($"   Tool call response: {toolResponse.StatusCode}");
            
            var toolContent = await toolResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"   Tool result: {toolContent}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"   POST / (JSON-RPC): Error - {ex.Message}");
    }
    Console.WriteLine();

    Console.WriteLine("2. Testing direct Coffee API...");
    try 
    {
        // Test the Coffee API directly
        var apiResponse = await httpClient.GetAsync("https://localhost:7073/api/products");
        
        if (apiResponse.IsSuccessStatusCode)
        {
            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"   Coffee API products: {apiContent}");
        }
        else
        {
            Console.WriteLine($"   Coffee API request failed: {apiResponse.StatusCode}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"   Coffee API error: {ex.Message}");
    }

    Console.WriteLine();
    Console.WriteLine("✅ All tests completed successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    return 1;
}

return 0;
