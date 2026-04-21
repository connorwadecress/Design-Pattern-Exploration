namespace DesignPatterns.Patterns.Strategy;

// The context. Holds a strategy and delegates the variable part (discount calculation)
// to it. This class never changes when a new strategy is added (Open/Closed Principle).
internal class Checkout
{
    private readonly IDiscountStrategy _strategy;

    public Checkout(IDiscountStrategy strategy) { _strategy = strategy; }

    public CheckoutResult CalculateTotal(Cart cart, Customer customer)
    {
        var discount = _strategy.CalculateDiscount(cart, customer);
        return new CheckoutResult(
            Subtotal: cart.Subtotal,
            Discount: discount,
            Total: cart.Subtotal - discount,
            StrategyName: _strategy.Name);
    }
}
