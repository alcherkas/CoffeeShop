using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Core.Services;

namespace CoffeeShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpGet("{sessionId}")]
    public async Task<IActionResult> GetBasket(string sessionId)
    {
        var items = await _basketService.GetBasketItemsAsync(sessionId);
        return Ok(items);
    }

    [HttpPost("{sessionId}/items")]
    public async Task<IActionResult> AddToBasket(string sessionId, [FromBody] AddToBasketRequest request)
    {
        var success = await _basketService.AddToBasketAsync(sessionId, request.ProductId, request.Quantity);
        if (!success)
        {
            return BadRequest("Failed to add item to basket");
        }
        return Ok();
    }

    [HttpPut("{sessionId}/items/{itemId}")]
    public async Task<IActionResult> UpdateQuantity(string sessionId, int itemId, [FromBody] UpdateQuantityRequest request)
    {
        var success = await _basketService.UpdateQuantityAsync(sessionId, itemId, request.Quantity);
        if (!success)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpDelete("{sessionId}/items/{itemId}")]
    public async Task<IActionResult> RemoveFromBasket(string sessionId, int itemId)
    {
        var success = await _basketService.RemoveFromBasketAsync(sessionId, itemId);
        if (!success)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpDelete("{sessionId}")]
    public async Task<IActionResult> ClearBasket(string sessionId)
    {
        await _basketService.ClearBasketAsync(sessionId);
        return Ok();
    }

    [HttpGet("{sessionId}/count")]
    public async Task<IActionResult> GetBasketCount(string sessionId)
    {
        var count = await _basketService.GetBasketCountAsync(sessionId);
        return Ok(new { count });
    }

    [HttpGet("{sessionId}/summary")]
    public async Task<IActionResult> GetBasketSummary(string sessionId)
    {
        var summary = await _basketService.GetBasketSummaryAsync(sessionId);
        return Ok(summary);
    }
}

public class AddToBasketRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
}

public class UpdateQuantityRequest
{
    public int Quantity { get; set; }
}