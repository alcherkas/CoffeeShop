using Microsoft.EntityFrameworkCore;
using CoffeeShop.Core.Data;
using CoffeeShop.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configure database
builder.Services.AddSingleton<CoffeeShopContext>(provider =>
{
    var options = new DbContextOptionsBuilder<CoffeeShopContext>()
        .UseInMemoryDatabase("CoffeeShopApiDb")
        .Options;
    return new CoffeeShopContext(options);
});

// Add business logic services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBasketService, BasketService>();

// Add CORS for web app and MCP server
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CoffeeShopContext>();
    context.Database.EnsureCreated();
    DataSeeder.SeedData(context);
    Console.WriteLine($"API database seeded with {context.Products.Count()} products");
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
