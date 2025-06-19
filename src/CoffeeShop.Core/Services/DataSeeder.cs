using CoffeeShop.Core.Data;
using CoffeeShop.Models;

namespace CoffeeShop.Core.Services;

public static class DataSeeder
{
    public static void SeedData(CoffeeShopContext context)
    {
        if (!context.Products.Any())
        {
            var products = new[]
            {
                new Product { Id = 1, Name = "Espresso", Description = "Strong and concentrated coffee", Price = 2.50m, Category = "Coffee", ImageUrl = "/images/espresso.jpg", IsAvailable = true },
                new Product { Id = 2, Name = "Americano", Description = "Espresso diluted with hot water", Price = 3.00m, Category = "Coffee", ImageUrl = "/images/americano.jpg", IsAvailable = true },
                new Product { Id = 3, Name = "Cappuccino", Description = "Espresso with steamed milk foam", Price = 3.75m, Category = "Coffee", ImageUrl = "/images/cappuccino.jpg", IsAvailable = true },
                new Product { Id = 4, Name = "Latte", Description = "Espresso with steamed milk", Price = 4.00m, Category = "Coffee", ImageUrl = "/images/latte.jpg", IsAvailable = true },
                new Product { Id = 5, Name = "Mocha", Description = "Espresso with chocolate and steamed milk", Price = 4.50m, Category = "Coffee", ImageUrl = "/images/mocha.jpg", IsAvailable = true },
                new Product { Id = 6, Name = "Macchiato", Description = "Espresso marked with a dollop of foamed milk", Price = 3.50m, Category = "Coffee", ImageUrl = "/images/macchiato.jpg", IsAvailable = true },
                new Product { Id = 7, Name = "Flat White", Description = "Espresso with microfoamed milk", Price = 3.80m, Category = "Coffee", ImageUrl = "/images/flatwhite.jpg", IsAvailable = true },
                new Product { Id = 8, Name = "Cold Brew", Description = "Smooth, cold-extracted coffee", Price = 3.25m, Category = "Cold Coffee", ImageUrl = "/images/coldbrew.jpg", IsAvailable = true },
                new Product { Id = 9, Name = "Iced Latte", Description = "Chilled espresso with cold milk", Price = 4.25m, Category = "Cold Coffee", ImageUrl = "/images/icedlatte.jpg", IsAvailable = true },
                new Product { Id = 10, Name = "Croissant", Description = "Buttery, flaky pastry", Price = 2.75m, Category = "Pastry", ImageUrl = "/images/croissant.jpg", IsAvailable = true },
                new Product { Id = 11, Name = "Blueberry Muffin", Description = "Fresh blueberry muffin", Price = 3.25m, Category = "Pastry", ImageUrl = "/images/muffin.jpg", IsAvailable = true },
                new Product { Id = 12, Name = "Bagel with Cream Cheese", Description = "Fresh bagel with cream cheese", Price = 3.50m, Category = "Food", ImageUrl = "/images/bagel.jpg", IsAvailable = true }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}