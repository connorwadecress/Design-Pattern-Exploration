namespace DesignPatterns.Patterns.Strategy;

/// <summary>
/// Demonstrates the Strategy pattern via shopping-cart discount calculation.
///
/// Four scenarios:
///   1. Same Checkout code running with four different strategies.
///   2. Runtime strategy swap via Checkout.SetStrategy(...).
///   3. Modern "lightweight" strategy using Func&lt;...&gt; instead of an interface.
///   4. Picking a strategy by runtime condition (what a real app typically does).
///
/// Interview talking points:
///   • Strategy = family of interchangeable algorithms behind one interface.
///   • The CONTEXT (Checkout) depends on the ABSTRACTION, never on concrete
///     strategies — Dependency Inversion + Open/Closed in action.
///   • Adding a new discount = new class implementing IDiscountStrategy.
///     Checkout never changes. This is OCP's canonical example.
///   • Strategy vs State: structurally identical, intent different. Strategy
///     is picked by the CLIENT ("how do I do this?"). State is picked by
///     the CONTEXT ITSELF ("what mode am I in right now?"), usually in
///     response to events.
///   • In modern C#, simple strategies can be expressed as Func&lt;...&gt; —
///     same pattern, less ceremony. Know both flavours.
/// </summary>
internal class StrategyDemo : IPatternDemo
{
    public string Name => "Strategy (discount calculator)";

    // A shared sample cart and customer for the demo.
    private static readonly Cart SampleCart = new(new[]
    {
        new CartItem("T-shirt",    25m, Quantity: 2),
        new CartItem("Hoodie",     60m, Quantity: 1),
        new CartItem("Cap",        15m, Quantity: 1),
    });

    private static readonly Customer GoldCustomer = new("Connor", LoyaltyTier.Gold);

    public void Run()
    {
        RunFourStrategies();
        Separator();
        RunRuntimeSwap();
        Separator();
        RunFuncVariant();
        Separator();
        RunPickByCondition();
        Separator();
        PrintSummary();
    }

    private static void RunFourStrategies()
    {
        Console.WriteLine("=== Scenario 1: Same Checkout, four different strategies ===");
        Console.WriteLine($"  Cart subtotal:  ${SampleCart.Subtotal}");
        Console.WriteLine($"  Customer:       {GoldCustomer.Name} ({GoldCustomer.Tier})");
        Console.WriteLine();

        IDiscountStrategy[] strategies =
        {
            new NoDiscountStrategy(),
            new PercentageDiscountStrategy(percent: 10m),
            new BulkDiscountStrategy(threshold: 100m, percent: 15m),
            new LoyaltyDiscountStrategy(),
        };

        foreach (var strategy in strategies)
        {
            var checkout = new Checkout(strategy);
            var result = checkout.CalculateTotal(SampleCart, GoldCustomer);
            Print(result);
        }
    }

    private static void RunRuntimeSwap()
    {
        Console.WriteLine("=== Scenario 2: Runtime strategy swap ===");
        Console.WriteLine("Same Checkout instance; strategy changed mid-flight.");
        Console.WriteLine();

        var checkout = new Checkout(new NoDiscountStrategy());
        Print(checkout.CalculateTotal(SampleCart, GoldCustomer));

        checkout.SetStrategy(new PercentageDiscountStrategy(25m));
        Print(checkout.CalculateTotal(SampleCart, GoldCustomer));

        checkout.SetStrategy(new LoyaltyDiscountStrategy());
        Print(checkout.CalculateTotal(SampleCart, GoldCustomer));
    }

    private static void RunFuncVariant()
    {
        Console.WriteLine("=== Scenario 3: Lightweight strategy via Func<> ===");
        Console.WriteLine("No interface, no class — just a delegate. Same pattern, less ceremony.");
        Console.WriteLine("Good for one-off strategies that don't need their own class.");
        Console.WriteLine();

        Func<Cart, Customer, decimal> flashSale = (cart, _) => cart.Subtotal * 0.30m;

        var discount = flashSale(SampleCart, GoldCustomer);
        var total = SampleCart.Subtotal - discount;

        Console.WriteLine($"  Strategy: Flash sale (Func<>)  Subtotal: ${SampleCart.Subtotal,7:0.00}   " +
                          $"Discount: ${discount,6:0.00}   Total: ${total,7:0.00}");
    }

    private static void RunPickByCondition()
    {
        Console.WriteLine("=== Scenario 4: Pick strategy by runtime condition ===");
        Console.WriteLine("A realistic scenario — the right strategy is chosen based on data.");
        Console.WriteLine();

        // Imagine these come from config, user preferences, or business rules.
        var flashSaleRunning = true;
        var customerIsPlatinum = GoldCustomer.Tier == LoyaltyTier.Platinum;

        IDiscountStrategy strategy =
            flashSaleRunning        ? new PercentageDiscountStrategy(30m) :
            customerIsPlatinum      ? new LoyaltyDiscountStrategy() :
            SampleCart.Subtotal > 100 ? new BulkDiscountStrategy() :
                                        new NoDiscountStrategy();

        Console.WriteLine($"  Selected strategy: {strategy.Name}");

        var result = new Checkout(strategy).CalculateTotal(SampleCart, GoldCustomer);
        Print(result);
    }

    private static void Print(CheckoutResult r)
    {
        Console.WriteLine(
            $"  Strategy: {r.StrategyName,-42}  " +
            $"Subtotal: ${r.Subtotal,7:0.00}   " +
            $"Discount: ${r.Discount,6:0.00}   " +
            $"Total: ${r.Total,7:0.00}");
    }

    private static void PrintSummary()
    {
        Console.WriteLine("=== Summary ===");
        Console.WriteLine("  Strategy replaces a big if/else or switch with a family of");
        Console.WriteLine("  interchangeable classes, all implementing the same interface.");
        Console.WriteLine();
        Console.WriteLine("  Payoffs:");
        Console.WriteLine("    • Each algorithm is testable in isolation.");
        Console.WriteLine("    • Adding a new discount = new class. Checkout never changes (OCP).");
        Console.WriteLine("    • The context depends on IDiscountStrategy, not on concrete types (DIP).");
        Console.WriteLine();
        Console.WriteLine("  Strategy vs State (next pattern):");
        Console.WriteLine("    • Strategy: CLIENT picks the algorithm. Rarely changes for a given context.");
        Console.WriteLine("    • State:    CONTEXT picks the next state itself in response to events.");
        Console.WriteLine();
        Console.WriteLine("  When NOT to use:");
        Console.WriteLine("    • Three tiny branches in one method — a `switch` expression is simpler.");
        Console.WriteLine("    • Algorithms that are unlikely to ever grow — don't pay the class-count cost.");
    }

    private static void Separator()
    {
        Console.WriteLine();
        Console.WriteLine(new string('-', 80));
        Console.WriteLine();
    }
}
