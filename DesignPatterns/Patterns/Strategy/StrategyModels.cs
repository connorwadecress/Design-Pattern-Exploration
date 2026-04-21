namespace DesignPatterns.Patterns.Strategy;

/// <summary>
/// Loyalty tier. Used by <see cref="LoyaltyDiscountStrategy"/> to decide
/// the discount percentage.
/// </summary>
internal enum LoyaltyTier { None, Silver, Gold, Platinum }

/// <summary>
/// A customer. Records give us immutability + value-equality for free.
/// </summary>
internal record Customer(string Name, LoyaltyTier Tier);

/// <summary>
/// A line in the shopping cart.
/// </summary>
internal record CartItem(string Name, decimal Price, int Quantity);

/// <summary>
/// The shopping cart. Not a record because we want computed properties.
/// Immutable from the outside (IReadOnlyList + computed getters).
/// </summary>
internal class Cart
{
    public IReadOnlyList<CartItem> Items { get; }

    public Cart(IEnumerable<CartItem> items)
    {
        Items = items.ToList();
    }

    /// <summary>Sum of price * quantity for every line.</summary>
    public decimal Subtotal => Items.Sum(i => i.Price * i.Quantity);

    /// <summary>Total number of units across all lines.</summary>
    public int TotalQuantity => Items.Sum(i => i.Quantity);
}

/// <summary>
/// Result of a checkout calculation — what the caller cares about after
/// the strategy has run.
/// </summary>
internal record CheckoutResult(
    decimal Subtotal,
    decimal Discount,
    decimal Total,
    string StrategyName);
