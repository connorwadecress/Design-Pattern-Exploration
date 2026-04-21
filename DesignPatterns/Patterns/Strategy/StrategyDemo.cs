namespace DesignPatterns.Patterns.Strategy;

internal class StrategyDemo : IPatternDemo
{
    public string Name => "Strategy";

    public void Run()
    {
        var cart = new Cart(new[]
        {
            new CartItem("T-shirt", 25m, 2),
            new CartItem("Hoodie", 60m, 1),
            new CartItem("Cap", 15m, 1),
        });
        var customer = new Customer("Connor", LoyaltyTier.Gold);

        Console.WriteLine($"Cart subtotal: ${cart.Subtotal}, customer tier: {customer.Tier}");
        Console.WriteLine();

        // Same Checkout class, different strategies - that's the whole pattern.
        IDiscountStrategy[] strategies =
        {
            new NoDiscountStrategy(),
            new PercentageDiscountStrategy(10m),
            new BulkDiscountStrategy(),
            new LoyaltyDiscountStrategy(),
        };

        foreach (var strategy in strategies)
        {
            var r = new Checkout(strategy).CalculateTotal(cart, customer);
            Console.WriteLine($"  {r.StrategyName,-25}  discount=${r.Discount,6:0.00}   total=${r.Total,7:0.00}");
        }

        Console.WriteLine();

        // Modern C#: a Func<> is a legitimate Strategy too - no interface or class needed.
        Console.WriteLine("Lightweight Func<> strategy (flash sale, 30% off):");
        Func<Cart, Customer, decimal> flashSale = (c, _) => c.Subtotal * 0.30m;
        var flashDiscount = flashSale(cart, customer);
        Console.WriteLine($"  discount=${flashDiscount,6:0.00}   total=${cart.Subtotal - flashDiscount,7:0.00}");
    }
}
