namespace DesignPatterns.Patterns.Strategy;

// The strategy interface - one method every discount algorithm implements.
internal interface IDiscountStrategy
{
    string Name { get; }
    decimal CalculateDiscount(Cart cart, Customer customer);
}

// No discount - doubles as a Null Object so callers never need a null check.
internal class NoDiscountStrategy : IDiscountStrategy
{
    public string Name => "No discount";
    public decimal CalculateDiscount(Cart cart, Customer customer) => 0m;
}

// Flat percentage off everything. Parameterised via the constructor -
// one class, many configured instances.
internal class PercentageDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _percent;
    public PercentageDiscountStrategy(decimal percent) { _percent = percent; }

    public string Name => $"{_percent}% off";
    public decimal CalculateDiscount(Cart cart, Customer customer)
        => cart.Subtotal * (_percent / 100m);
}

// Bulk discount - only applies when the cart subtotal crosses a threshold.
// The eligibility rule lives inside the strategy, not in the context.
internal class BulkDiscountStrategy : IDiscountStrategy
{
    public string Name => "Bulk (15% when > $100)";
    public decimal CalculateDiscount(Cart cart, Customer customer)
        => cart.Subtotal > 100m ? cart.Subtotal * 0.15m : 0m;
}

// Tier-based discount. Uses customer data, not just cart data -
// which is why the interface method takes both.
internal class LoyaltyDiscountStrategy : IDiscountStrategy
{
    public string Name => "Loyalty tier";
    public decimal CalculateDiscount(Cart cart, Customer customer)
    {
        var percent = customer.Tier switch
        {
            LoyaltyTier.Silver => 5m,
            LoyaltyTier.Gold => 10m,
            LoyaltyTier.Platinum => 20m,
            _ => 0m
        };
        return cart.Subtotal * (percent / 100m);
    }
}
