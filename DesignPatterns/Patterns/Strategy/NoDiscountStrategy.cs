namespace DesignPatterns.Patterns.Strategy;

/// <summary>
/// The "null" discount — no discount at all. Useful as a sensible default
/// and as an example of the "Null Object" pattern applied to a strategy:
/// callers never have to check for `strategy == null` — they just use this.
/// </summary>
internal class NoDiscountStrategy : IDiscountStrategy
{
    public string Name => "No discount";

    public decimal CalculateDiscount(Cart cart, Customer customer) => 0m;
}
