using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Web.Services;

namespace CoffeeShopApp.Controllers;

public class BasketController : Controller
{
    private readonly IApiBasketService _basketService;

    public BasketController(IApiBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<IActionResult> Index()
    {
        var sessionId = GetOrCreateSessionId();
        Console.WriteLine($"Basket Index - Session ID: {sessionId}");
        
        var basketItems = await _basketService.GetBasketItemsAsync(sessionId);
            
        Console.WriteLine($"Basket Index - Found {basketItems.Count()} items for session {sessionId}");

        return View(basketItems);
    }

    [HttpPost]
    public async Task<IActionResult> AddToBasket(int productId, int quantity = 1)
    {
        Console.WriteLine($"AddToBasket called: ProductId={productId}, Quantity={quantity}");
        
        var sessionId = GetOrCreateSessionId();
        Console.WriteLine($"Session ID: {sessionId}");
        
        var success = await _basketService.AddToBasketAsync(sessionId, productId, quantity);
        
        if (!success)
        {
            Console.WriteLine("Failed to add to basket");
            return NotFound();
        }

        Console.WriteLine("Successfully added to basket");
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int id, int quantity)
    {
        var sessionId = GetOrCreateSessionId();
        var success = await _basketService.UpdateQuantityAsync(sessionId, id, quantity);
        
        if (!success)
        {
            return NotFound();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromBasket(int id)
    {
        var sessionId = GetOrCreateSessionId();
        await _basketService.RemoveFromBasketAsync(sessionId, id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> GetBasketCount()
    {
        var sessionId = GetOrCreateSessionId();
        var count = await _basketService.GetBasketCountAsync(sessionId);

        return Json(new { count });
    }

    [HttpGet]
    public async Task<IActionResult> TestAdd(int productId = 1)
    {
        return await AddToBasket(productId, 1);
    }


    private string GetOrCreateSessionId()
    {
        var existingSessionId = HttpContext.Session.GetString("SessionId");
        Console.WriteLine($"GetOrCreateSessionId - Existing session: {existingSessionId ?? "NULL"}");
        
        if (existingSessionId == null)
        {
            var newSessionId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("SessionId", newSessionId);
            Console.WriteLine($"GetOrCreateSessionId - Created new session: {newSessionId}");
            return newSessionId;
        }
        
        Console.WriteLine($"GetOrCreateSessionId - Using existing session: {existingSessionId}");
        return existingSessionId;
    }
}