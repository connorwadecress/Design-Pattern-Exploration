namespace DesignPatterns.Patterns.Strategy;

internal enum LoyaltyTier { None, Silver, Gold, Platinum }

internal record Customer(string Name, LoyaltyTier Tier);

internal record CartItem(string Name, decimal Price, int Quantity);

internal class Cart
{
    public IReadOnlyList<CartItem> Items { get; }
    public Cart(IEnumerable<CartItem> items) { Items = items.ToList(); }

    public decimal Subtotal => Items.Sum(i => i.Price * i.Quantity);
}

internal record CheckoutResult(decimal Subtotal, decimal Discount, decimal Total, string StrategyName);
