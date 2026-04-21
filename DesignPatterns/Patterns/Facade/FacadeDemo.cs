namespace DesignPatterns.Patterns.Facade;

/// <summary>
/// Demonstrates the Facade pattern via an order-checkout example.
///
/// Three scenarios:
///   1. Happy path — one call, four subsystems cooperating.
///   2. Payment failure — watch the facade roll back the reservation
///      without the caller writing any rollback code.
///   3. Bypass — advanced caller going straight to a subsystem service,
///      proving Facade doesn't BLOCK direct access; it just makes the
///      common path easier.
///
/// Interview talking points:
///   • Facade provides a simple, domain-oriented interface to a complex subsystem.
///   • It's ADDITIVE — you don't have to go through it if you don't want to.
///   • It absorbs cross-cutting concerns (ordering, rollback, logging) so every
///     caller doesn't reimplement them.
///   • Contrast with Mediator: Facade is one-directional (client -> facade
///     -> subsystem, which doesn't know the facade exists). Mediator is
///     multi-directional (peers send messages to a mediator that coordinates them).
///   • Contrast with Adapter: Adapter is about COMPATIBILITY (fitting the
///     wrong-shaped interface into a hole); Facade is about EASE OF USE
///     (reducing a big interface to a small one).
/// </summary>
internal class FacadeDemo : IPatternDemo
{
    public string Name => "Facade (order checkout)";

    public void Run()
    {
        RunHappyPath();
        Separator();
        RunPaymentFailure();
        Separator();
        RunBypass();
        Separator();
        PrintSummary();
    }

    private static void RunHappyPath()
    {
        Console.WriteLine("=== Scenario 1: Happy path ===");
        Console.WriteLine("One line of client code. Four subsystems cooperate behind the scenes.");
        Console.WriteLine();

        var facade = BuildFacade(simulatePaymentFailure: false);
        var customer = new Customer("Connor", "connor@example.com", "12 Fake St, Cape Town");
        var item = new Item("WIDGET-01", "Blue widget", 49.99m);

        var result = facade.PlaceOrder(customer, item);

        Console.WriteLine();
        Console.WriteLine($"  Client sees:  Succeeded={result.Succeeded}, Tracking={result.TrackingNumber}");
    }

    private static void RunPaymentFailure()
    {
        Console.WriteLine("=== Scenario 2: Payment failure (automatic rollback) ===");
        Console.WriteLine("Watch the facade release the inventory reservation when payment is declined.");
        Console.WriteLine("The caller writes zero lines of rollback logic — that's the facade's job.");
        Console.WriteLine();

        var facade = BuildFacade(simulatePaymentFailure: true);
        var customer = new Customer("Alice", "alice@example.com", "99 Example Rd, Johannesburg");
        var item = new Item("GADGET-07", "Red gadget", 149.99m);

        var result = facade.PlaceOrder(customer, item);

        Console.WriteLine();
        Console.WriteLine($"  Client sees:  Succeeded={result.Succeeded}, Reason=\"{result.FailureReason}\"");
    }

    private static void RunBypass()
    {
        Console.WriteLine("=== Scenario 3: Direct subsystem access (bypass) ===");
        Console.WriteLine("Facade doesn't PREVENT direct access to the subsystem.");
        Console.WriteLine("If you need something off the common path, go straight to the service.");
        Console.WriteLine();

        IInventoryService inventory = new InventoryService();
        var inStock = inventory.CheckStock(new Item("SPECIAL-03", "Rare item", 999m));

        Console.WriteLine();
        Console.WriteLine($"  Direct call to IInventoryService.CheckStock returned: {inStock}");
        Console.WriteLine("  No facade involved. Still valid usage.");
    }

    private static OrderCheckoutFacade BuildFacade(bool simulatePaymentFailure)
    {
        // In a real app this wiring happens in the DI container, not by hand.
        return new OrderCheckoutFacade(
            inventory: new InventoryService(),
            payment: new PaymentService(simulatePaymentFailure),
            shipping: new ShippingService(),
            notification: new NotificationService());
    }

    private static void PrintSummary()
    {
        Console.WriteLine("=== Summary ===");
        Console.WriteLine("  Facade gives callers a small, domain-oriented front door to a");
        Console.WriteLine("  complex subsystem. Client code goes from:");
        Console.WriteLine();
        Console.WriteLine("      checkStock, reserve, charge, (rollback if needed),");
        Console.WriteLine("      createShipment, sendEmail");
        Console.WriteLine();
        Console.WriteLine("  …to a single:");
        Console.WriteLine();
        Console.WriteLine("      facade.PlaceOrder(customer, item)");
        Console.WriteLine();
        Console.WriteLine("  The facade absorbs the choreography and cross-cutting concerns.");
        Console.WriteLine();
        Console.WriteLine("  Contrast quick-reference:");
        Console.WriteLine("    Facade vs Adapter    -> ease-of-use vs compatibility");
        Console.WriteLine("    Facade vs Mediator   -> one-directional client->subsystem vs peers<->mediator");
        Console.WriteLine("    Facade vs God Object -> mitigate by keeping facades small & per-context");
    }

    private static void Separator()
    {
        Console.WriteLine();
        Console.WriteLine(new string('-', 64));
        Console.WriteLine();
    }
}
