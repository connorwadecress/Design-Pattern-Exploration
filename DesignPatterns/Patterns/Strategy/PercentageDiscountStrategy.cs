namespace DesignPatterns.Patterns.Strategy;

/// <summary>
/// Flat percentage discount — e.g. "10% off everything".
///
/// Demonstrates a PARAMETERISED strategy: the algorithm is the same, but
/// its behaviour is configured via the constructor. Same class can serve
/// as a 5%-off discount, a 20%-off discount, a 50%-off discount — just
/// construct it with a different percent.
/// </summary>
internal class PercentageDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _percent;

    public PercentageDiscountStrategy(decimal percent)
    {
        if (percent < 0 || percent > 100)
            throw new ArgumentOutOfRangeException(nameof(percent), "Must be 0..100.");
        _percent = percent;
    }

    public string Name => $"Flat {_percent}% off";

    public decimal CalculateDiscount(Cart cart, Customer customer)
        => cart.Subtotal * (_percent / 100m);
}
