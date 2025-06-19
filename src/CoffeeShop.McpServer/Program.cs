using CoffeeShop.McpServer.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7074;
});

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithTools<GetProductsTool>()
    .WithTools<GetProductTool>()
    .WithTools<GetCategororiesTool>()
    .WithTools<GetBasketTool>()
    .WithTools<AddToBasketTool>();

var app = builder.Build();

app.UseHttpsRedirection();

// Try alternative MCP mapping
try
{
    app.MapMcp();
}
catch (Exception ex)
{
    Console.WriteLine($"MapMcp failed: {ex.Message}");
    
    // Manual endpoint mapping as fallback
    app.MapPost("/", async (HttpContext context) =>
    {
        return Results.Json(new { error = "MCP endpoints not properly configured", message = ex.Message });
    });
    
    app.MapGet("/sse", async (HttpContext context) =>
    {
        context.Response.Headers.Add("Content-Type", "text/event-stream");
        context.Response.Headers.Add("Cache-Control", "no-cache");
        context.Response.Headers.Add("Connection", "keep-alive");
        
        await context.Response.WriteAsync("data: {\"jsonrpc\":\"2.0\",\"id\":1,\"result\":{\"tools\":[]}}\n\n");
        await context.Response.Body.FlushAsync();
        
        // Keep connection alive
        await Task.Delay(TimeSpan.FromHours(1), context.RequestAborted);
    });
}

app.Run();
