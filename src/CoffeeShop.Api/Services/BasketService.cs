using Microsoft.EntityFrameworkCore;
using CoffeeShop.Core.Data;
using CoffeeShop.Models;

namespace CoffeeShop.Core.Services;

public class BasketService : IBasketService
{
    private readonly CoffeeShopContext _context;
    private const decimal TaxRate = 0.08m;

    public BasketService(CoffeeShopContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BasketItem>> GetBasketItemsAsync(string sessionId)
    {
        return await _context.BasketItems
            .Include(b => b.Product)
            .Where(b => b.SessionId == sessionId)
            .ToListAsync();
    }

    public async Task<bool> AddToBasketAsync(string sessionId, int productId, int quantity = 1)
    {
        try
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || !product.IsAvailable)
                return false;

            var existingItem = await _context.BasketItems
                .FirstOrDefaultAsync(b => b.ProductId == productId && b.SessionId == sessionId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var basketItem = new BasketItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    SessionId = sessionId
                };
                _context.BasketItems.Add(basketItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateQuantityAsync(string sessionId, int basketItemId, int quantity)
    {
        try
        {
            var basketItem = await _context.BasketItems
                .FirstOrDefaultAsync(b => b.Id == basketItemId && b.SessionId == sessionId);

            if (basketItem == null)
                return false;

            if (quantity <= 0)
            {
                _context.BasketItems.Remove(basketItem);
            }
            else
            {
                basketItem.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveFromBasketAsync(string sessionId, int basketItemId)
    {
        try
        {
            var basketItem = await _context.BasketItems
                .FirstOrDefaultAsync(b => b.Id == basketItemId && b.SessionId == sessionId);

            if (basketItem != null)
            {
                _context.BasketItems.Remove(basketItem);
                await _context.SaveChangesAsync();
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task ClearBasketAsync(string sessionId)
    {
        var basketItems = await _context.BasketItems
            .Where(b => b.SessionId == sessionId)
            .ToListAsync();

        _context.BasketItems.RemoveRange(basketItems);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetBasketCountAsync(string sessionId)
    {
        return await _context.BasketItems
            .Where(b => b.SessionId == sessionId)
            .SumAsync(b => b.Quantity);
    }

    public async Task<decimal> GetBasketTotalAsync(string sessionId)
    {
        var subtotal = await _context.BasketItems
            .Where(b => b.SessionId == sessionId)
            .SumAsync(b => b.Quantity * b.UnitPrice);

        return subtotal * (1 + TaxRate);
    }

    public async Task<BasketSummary> GetBasketSummaryAsync(string sessionId)
    {
        var items = await GetBasketItemsAsync(sessionId);
        var subtotal = items.Sum(i => i.Quantity * i.UnitPrice);
        var tax = subtotal * TaxRate;
        var total = subtotal + tax;

        return new BasketSummary
        {
            Items = items,
            TotalItems = items.Sum(i => i.Quantity),
            Subtotal = subtotal,
            Tax = tax,
            Total = total
        };
    }
}