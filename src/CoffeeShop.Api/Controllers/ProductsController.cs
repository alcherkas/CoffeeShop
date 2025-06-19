using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Core.Services;

namespace CoffeeShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] string? category = null, [FromQuery] string? search = null)
    {
        if (!string.IsNullOrEmpty(category))
        {
            var categoryProducts = await _productService.GetProductsByCategoryAsync(category);
            return Ok(categoryProducts);
        }

        if (!string.IsNullOrEmpty(search))
        {
            var searchResults = await _productService.SearchProductsAsync(search);
            return Ok(searchResults);
        }

        var products = await _productService.GetAvailableProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _productService.GetCategoriesAsync();
        return Ok(categories);
    }
}