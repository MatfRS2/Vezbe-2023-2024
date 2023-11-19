using Microsoft.AspNetCore.Http.Features;

namespace Basket.API.Entities;

public class ShoppingCart
{
    public ShoppingCart(string username)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
    }

    public IEnumerable<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

    public string Username { get; set; }

    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);

}