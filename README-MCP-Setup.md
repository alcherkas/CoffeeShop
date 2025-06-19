# Coffee Shop MCP Server Setup

The MCP server supports dual modes:
1. **Web API mode**: HTTP API at `https://localhost:7074`
2. **MCP stdio mode**: Direct integration with Claude Desktop via stdin/stdout

## Architecture

- **Coffee Shop API**: Business logic API at `https://localhost:7073`
- **MCP Server**: Dual-mode server (Web API + stdio)
- **Web App**: Frontend at `https://localhost:5000` (or assigned port)

## Running the Services

1. **Start the Coffee Shop API** (required first):
   ```bash
   cd src/CoffeeShop.Api
   dotnet run
   ```

2. **Start the MCP Server**:
   
   **For Web API mode:**
   ```bash
   cd src/CoffeeShop.McpServer
   dotnet run
   ```
   
   **For MCP stdio mode (Claude Desktop):**
   ```bash
   cd src/CoffeeShop.McpServer
   dotnet run -- --stdio
   ```

3. **Start the Web App** (optional):
   ```bash
   cd src/CoffeeShop.Web
   dotnet run
   ```

## Claude Desktop Configuration

### Option 1: JSON-RPC over HTTP (Recommended)

**Modern approach using JSON-RPC 2.0 over HTTP**

1. First install Node.js bridge dependencies:
   ```bash
   npm install
   ```

2. Configure Claude Desktop:
   ```json
   {
     "mcpServers": {
       "coffee-shop": {
         "command": "node",
         "args": [
           "/Users/aleksandrcherkas/Documents/GitHub/CoffeeShop/mcp-http-bridge.js"
         ],
         "env": {
           "MCP_SERVER_URL": "https://localhost:7074"
         }
       }
     }
   }
   ```

### Option 2: Direct stdio (.NET Native)

**Pure .NET solution (no Node.js required)**

```json
{
  "mcpServers": {
    "coffee-shop": {
      "command": "dotnet",
      "args": [
        "run",
        "--project", 
        "/Users/aleksandrcherkas/Documents/GitHub/CoffeeShop/src/CoffeeShop.McpServer",
        "--",
        "--stdio"
      ],
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

**Update the path** to match your actual project location.

## Current Status

The MCP C# SDK is in preview and the API is still evolving. I've implemented a working solution using:

- **Dual-mode architecture**: Web API + stdio support
- **JSON-RPC 2.0**: Standard protocol for HTTP API integration  
- **Native MCP stdio**: Direct Claude Desktop integration
- **API-first design**: Business logic stays in separate API service

This approach provides maximum compatibility while the official SDK matures.

## Alternative: Pre-built Executable

You can also build and use a standalone executable:

```bash
cd src/CoffeeShop.McpServer
dotnet publish -c Release -r win-x64 --self-contained
```

Then use in Claude Desktop config:
```json
{
  "mcpServers": {
    "coffee-shop": {
      "command": "/path/to/published/CoffeeShop.McpServer.exe",
      "args": ["--stdio"]
    }
  }
}
```

## Testing MCP Connection

### Test JSON-RPC Mode
```bash
# Run the test script
./test-jsonrpc.sh

# Or test manually
curl -X POST https://localhost:7074 \
  -H "Content-Type: application/json" \
  -d '{
    "jsonrpc": "2.0",
    "id": "1",
    "method": "initialize", 
    "params": {}
  }' -k
```

### Test stdio Mode
```bash
cd src/CoffeeShop.McpServer
echo '{"id":"1","method":"initialize","params":{}}' | dotnet run -- --stdio
```

### Test HTTP Bridge
```bash
# Start the bridge in HTTP mode for testing
node mcp-http-bridge.js --http
# Bridge will run on port 3001
```

## Available MCP Tools

The MCP server provides these tools for AI assistants:

- `get_products` - Get all available products (with optional category/search filters)
- `get_product` - Get details of a specific product by ID
- `get_categories` - Get all available product categories
- `add_to_basket` - Add a product to the shopping basket
- `get_basket` - Get current basket contents for a session
- `update_basket_item` - Update quantity of an item in the basket
- `remove_from_basket` - Remove an item from the basket
- `get_basket_summary` - Get basket summary with totals and tax
- `clear_basket` - Clear all items from the basket

## Example MCP Request

```json
{
  "id": "1",
  "method": "tools/call",
  "params": {
    "name": "get_products",
    "arguments": {
      "category": "Coffee"
    }
  }
}
```

## Troubleshooting

1. **Connection refused**: Ensure the Coffee Shop API is running on port 7073
2. **CORS errors**: The MCP server has CORS enabled for development
3. **SSL certificate errors**: Use `--ignore-certificate-errors` flag if needed during development

## Security Note

This configuration is for development only. For production use:
- Enable proper SSL certificates
- Configure authentication/authorization
- Restrict CORS to specific origins
- Use environment variables for configuration