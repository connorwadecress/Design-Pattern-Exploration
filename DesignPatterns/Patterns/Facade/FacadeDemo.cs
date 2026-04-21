namespace DesignPatterns.Patterns.Facade;

internal class FacadeDemo : IPatternDemo
{
    public string Name => "Facade";

    public void Run()
    {
        var customer = new Customer("Connor", "connor@example.com");
        var item = new Item("WIDGET-01", 49.99m);

        Console.WriteLine("Happy path - one call, four subsystems cooperating:");
        var facade1 = BuildFacade(simulateFailure: false);
        var r1 = facade1.PlaceOrder(customer, item);
        Console.WriteLine($"  result: succeeded={r1.Succeeded}, tracking={r1.TrackingNumber}");
        Console.WriteLine();

        Console.WriteLine("Payment failure - facade rolls back the reservation:");
        var facade2 = BuildFacade(simulateFailure: true);
        var r2 = facade2.PlaceOrder(customer, item);
        Console.WriteLine($"  result: succeeded={r2.Succeeded}, reason={r2.FailureReason}");
    }

    private static OrderCheckoutFacade BuildFacade(bool simulateFailure)
        => new(new InventoryService(),
               new PaymentService(simulateFailure),
               new ShippingService(),
               new NotificationService());
}
