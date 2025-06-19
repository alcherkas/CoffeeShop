using CoffeeShop.Models;

namespace CoffeeShop.Core.Services;

public interface IBasketService
{
    Task<IEnumerable<BasketItem>> GetBasketItemsAsync(string sessionId);
    Task<bool> AddToBasketAsync(string sessionId, int productId, int quantity = 1);
    Task<bool> UpdateQuantityAsync(string sessionId, int basketItemId, int quantity);
    Task<bool> RemoveFromBasketAsync(string sessionId, int basketItemId);
    Task ClearBasketAsync(string sessionId);
    Task<int> GetBasketCountAsync(string sessionId);
    Task<decimal> GetBasketTotalAsync(string sessionId);
    Task<BasketSummary> GetBasketSummaryAsync(string sessionId);
}