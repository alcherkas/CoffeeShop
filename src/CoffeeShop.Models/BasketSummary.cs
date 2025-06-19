namespace CoffeeShop.Models;

public class BasketSummary
{
    public IEnumerable<BasketItem> Items { get; set; } = [];
    public int TotalItems { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
}