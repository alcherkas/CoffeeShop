# Coffee Shop Solution

A comprehensive coffee shop management system with web interface, REST API, and MCP (Model Context Protocol) server for AI assistant integration.

## 🏗️ Project Structure

```
CoffeeShop/
├── CoffeeShop.sln                      # Solution file
├── src/
│   ├── CoffeeShop.Models/              # Shared data models
│   │   ├── Product.cs                  # Product model
│   │   ├── BasketItem.cs              # Shopping cart item model
│   │   └── BasketSummary.cs           # Order summary model
│   ├── CoffeeShop.Api/                 # REST API (port 7073)
│   │   ├── Controllers/                # API controllers
│   │   ├── Services/                   # Business services
│   │   ├── Data/                       # Entity Framework context
│   │   └── Program.cs                  # API startup
│   ├── CoffeeShop.Web/                 # Web application
│   │   ├── Controllers/                # MVC controllers
│   │   ├── Views/                      # Razor views
│   │   └── Program.cs                  # Web app startup
│   ├── CoffeeShop.McpServer/           # MCP server (port 7074)
│   │   ├── Tools/                      # MCP tool implementations
│   │   └── Program.cs                  # MCP server startup
│   └── CoffeeShop.McpClient/           # Test client for MCP server
│       └── Program.cs                  # Client test application
├── claude_desktop_config.json          # Claude Desktop configuration
└── README.md
```

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- Any IDE with C# support (Visual Studio, VS Code, Rider)

### Building the Solution
```bash
# Build all projects
dotnet build CoffeeShop.sln
```

### Running the Applications

1. **Start the REST API** (required for MCP server):
```bash
cd src/CoffeeShop.Api
dotnet run
# Available at: https://localhost:7073
```

2. **Start the Web Application** (optional):
```bash
cd src/CoffeeShop.Web
dotnet run
# Navigate to: http://localhost:5000
```

3. **Start the MCP Server**:
```bash
cd src/CoffeeShop.McpServer
dotnet run --launch-profile https
# Available at: https://localhost:7074
```

### Testing the MCP Server
```bash
cd src/CoffeeShop.McpClient
dotnet run
# Tests all MCP endpoints and tools
```

## 🌐 Web Application Features

- **Product Catalog**: Browse coffee, cold drinks, pastries, and food
- **Shopping Basket**: Add items, update quantities, remove items
- **Session Management**: Basket persists during user session
- **Responsive Design**: Bootstrap-based mobile-friendly interface
- **Tax Calculation**: 8% tax rate applied to orders

### Available Routes
- `/` - Product catalog homepage
- `/Basket` - Shopping basket view
- `/Home/TestProducts` - Product data verification endpoint

## 🤖 MCP Server Features

Built with **ModelContextProtocol v0.2.0-preview.3** package, providing AI assistants with tools to manage the coffee shop:

### Available MCP Tools
- **`GetProducts`** - Browse all products with optional category/search filtering
- **`GetProduct`** - Get specific product details by ID
- **`GetCategories`** - List all product categories
- **`GetBasket`** - View current basket contents by session
- **`GetBasketSummary`** - Get basket totals with pricing
- **`AddToBasket`** - Add products to shopping cart

### Claude Desktop Integration

Add to your Claude Desktop configuration file:

```json
{
  "mcpServers": {
    "coffee-shop": {
      "url": "https://localhost:7074"
    }
  }
}
```

**Steps:**
1. Ensure the Coffee API is running on port 7073
2. Start the MCP server on port 7074
3. Add the configuration to Claude Desktop
4. Restart Claude Desktop
5. Coffee shop tools will be available in Claude

### MCP Server Architecture
- **HTTP Transport**: Uses Server-Sent Events (SSE) for real-time communication
- **JSON-RPC Protocol**: Standard MCP protocol implementation
- **Tool Registration**: Automatic discovery via attributes (`[McpServerToolType]`, `[McpServerTool]`)
- **Error Handling**: Comprehensive error responses with detailed messages

## 🗄️ Data Management

### In-Memory Database
- Uses Entity Framework Core with in-memory provider
- Shared singleton database instance across web app and MCP server
- Pre-seeded with 12 products across 4 categories

### Sample Products
- **Coffee**: Espresso, Americano, Cappuccino, Latte, Mocha, Macchiato, Flat White
- **Cold Coffee**: Cold Brew, Iced Latte
- **Pastry**: Croissant, Blueberry Muffin
- **Food**: Bagel with Cream Cheese

## 🧮 Business Logic (CoffeeShop.Core)

### Services
- **IProductService**: Product catalog operations
- **IBasketService**: Shopping cart management with tax calculation
- **DataSeeder**: Consistent product data initialization

### Models
- **Product**: Coffee shop items with pricing and categorization
- **BasketItem**: Shopping cart entries with session tracking
- **BasketSummary**: Order totals with tax breakdown

## 🔧 Development

### Adding New Products
Update `DataSeeder.SeedData()` in `src/CoffeeShop.Core/Services/DataSeeder.cs`

### Extending MCP Tools
Add new tool definitions in `CoffeeShopMcpServer.HandleToolsListAsync()` and implementations in `ToolCallHandler`

### Customizing Web UI
Modify Razor views in `src/CoffeeShop.Web/Views/`

## 📝 Example MCP Tool Usage

### Tool Call Examples (JSON-RPC):
```json
// List all tools
{
  "jsonrpc": "2.0",
  "id": 1,
  "method": "tools/list",
  "params": {}
}

// Get all coffee products
{
  "jsonrpc": "2.0",
  "id": 2,
  "method": "tools/call",
  "params": {
    "name": "GetProducts",
    "arguments": { "category": "Coffee" }
  }
}

// Add item to basket
{
  "jsonrpc": "2.0",
  "id": 3,
  "method": "tools/call",
  "params": {
    "name": "AddToBasket",
    "arguments": {
      "sessionId": "user123",
      "productId": 1,
      "quantity": 2
    }
  }
}
```

### Sample Responses:
```json
// GetCategories response
{
  "result": {
    "content": [
      {
        "type": "text",
        "text": "[\"Coffee\", \"Cold Coffee\", \"Food\", \"Pastry\"]"
      }
    ],
    "isError": false
  },
  "id": 2,
  "jsonrpc": "2.0"
}
```

## 🏗️ Architecture Benefits

- **Microservices Architecture**: Separate API and MCP server for scalability
- **Shared Models**: Common data structures across all projects
- **HTTP API Integration**: MCP server consumes REST API endpoints
- **Real-time Communication**: SSE transport for responsive AI interactions
- **Testable Design**: Comprehensive test client for validation
- **AI-Ready**: Full MCP protocol compliance for AI assistant integration