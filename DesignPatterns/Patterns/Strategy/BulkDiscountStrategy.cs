namespace DesignPatterns.Patterns.Strategy;

/// <summary>
/// Bulk-order discount: a percentage off, but only once the cart subtotal
/// crosses a threshold.
///
/// Demonstrates a strategy whose algorithm has its own INTERNAL RULE
/// (the threshold check) that the caller doesn't need to know about.
/// That's the point of Strategy: the context just asks "what's the discount?"
/// and doesn't care what logic produced the answer.
/// </summary>
internal class BulkDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _threshold;
    private readonly decimal _percent;

    public BulkDiscountStrategy(decimal threshold = 100m, decimal percent = 15m)
    {
        _threshold = threshold;
        _percent = percent;
    }

    public string Name => $"Bulk ({_percent}% off when subtotal > ${_threshold})";

    public decimal CalculateDiscount(Cart cart, Customer customer)
    {
        // If under the threshold, no discount — the strategy contains
        // its own eligibility rule, not just an arithmetic formula.
        if (cart.Subtotal <= _threshold) return 0m;
        return cart.Subtotal * (_percent / 100m);
    }
}
