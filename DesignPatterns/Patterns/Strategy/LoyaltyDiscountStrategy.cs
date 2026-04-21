namespace DesignPatterns.Patterns.Strategy;

/// <summary>
/// Tiered discount based on the customer's loyalty tier.
///
/// Demonstrates a strategy that uses MORE than the primary input —
/// it reaches into the Customer argument. This is why the strategy
/// interface's method takes both Cart AND Customer: different
/// strategies need different inputs, so the signature is uniform
/// enough to carry what any of them might need.
/// </summary>
internal class LoyaltyDiscountStrategy : IDiscountStrategy
{
    public string Name => "Loyalty tier";

    public decimal CalculateDiscount(Cart cart, Customer customer)
    {
        // Switch expression — C# 8+ syntax, much cleaner than an if/else chain.
        var percent = customer.Tier switch
        {
            LoyaltyTier.None     => 0m,
            LoyaltyTier.Silver   => 5m,
            LoyaltyTier.Gold     => 10m,
            LoyaltyTier.Platinum => 20m,
            _                    => 0m
        };

        return cart.Subtotal * (percent / 100m);
    }
}
