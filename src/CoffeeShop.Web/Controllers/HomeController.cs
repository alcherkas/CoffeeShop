using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Web.Services;

namespace CoffeeShopApp.Controllers;

public class HomeController : Controller
{
    private readonly IApiProductService _productService;

    public HomeController(IApiProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        Console.WriteLine("HomeController.Index called");
        
        var products = await _productService.GetAvailableProductsAsync();

        Console.WriteLine($"Found {products.Count()} products");
        foreach (var product in products.Take(3))
        {
            Console.WriteLine($"Product: {product.Name} - ${product.Price}");
        }

        return View(products);
    }

    public async Task<IActionResult> Product(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null || !product.IsAvailable)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> TestProducts()
    {
        var allProducts = await _productService.GetAvailableProductsAsync();
        return Json(new
        {
            TotalProducts = allProducts.Count(),
            AvailableProducts = allProducts.Count(p => p.IsAvailable),
            Products = allProducts.Select(p => new { p.Id, p.Name, p.Price, p.Category, p.IsAvailable })
        });
    }

    [HttpGet]
    public async Task<IActionResult> TestSimple()
    {
        var count = (await _productService.GetAvailableProductsAsync()).Count();
        return Content($"Products in database: {count}");
    }
}