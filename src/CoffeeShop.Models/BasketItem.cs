namespace CoffeeShop.Models;

public class BasketItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string SessionId { get; set; } = string.Empty;
}