namespace DesignPatterns.Patterns.Strategy;

/// <summary>
/// The CONTEXT.
///
/// Holds a reference to an <see cref="IDiscountStrategy"/> and delegates the
/// variable part of its work (working out a discount) to that strategy.
///
/// Notice:
///   • The strategy is injected via the constructor — Dependency Inversion.
///   • Checkout has NO idea which concrete strategy it's holding.
///   • Checkout itself does not change when new strategies are added — that's
///     Open/Closed Principle at work.
///   • A SetStrategy method is provided to show runtime swapping. In many
///     real apps you'd just construct a new Checkout with a different
///     strategy instead — both are valid.
/// </summary>
internal class Checkout
{
    private IDiscountStrategy _strategy;

    public Checkout(IDiscountStrategy strategy)
    {
        _strategy = strategy;
    }

    /// <summary>
    /// Swap the strategy in place. Useful for demo purposes; in real code
    /// you'd more often construct a new Checkout with the right strategy.
    /// </summary>
    public void SetStrategy(IDiscountStrategy strategy)
    {
        _strategy = strategy;
    }

    /// <summary>
    /// Compute the checkout total using whichever strategy is currently set.
    /// This method's body never changes as new strategies are added.
    /// </summary>
    public CheckoutResult CalculateTotal(Cart cart, Customer customer)
    {
        var subtotal = cart.Subtotal;
        var discount = _strategy.CalculateDiscount(cart, customer);
        var total = subtotal - discount;

        return new CheckoutResult(
            Subtotal: subtotal,
            Discount: discount,
            Total: total,
            StrategyName: _strategy.Name);
    }
}
