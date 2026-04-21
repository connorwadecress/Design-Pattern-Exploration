namespace DesignPatterns.Patterns.Facade;

// Simple data types used by the checkout.
internal record Customer(string Name, string Email);
internal record Item(string Sku, decimal Price);
internal record OrderResult(bool Succeeded, string? TrackingNumber, string? FailureReason);

// The four subsystem services the facade orchestrates.
// Each is tiny - the point of the pattern is that coordinating them is what's painful,
// not any one service on its own.

internal interface IInventoryService
{
    string Reserve(Item item);
    void Release(string reservationId);
}

internal class InventoryService : IInventoryService
{
    public string Reserve(Item item)
    {
        var id = $"RES-{Guid.NewGuid().ToString()[..6]}";
        Console.WriteLine($"  [Inventory]    reserved {item.Sku} ({id})");
        return id;
    }

    public void Release(string reservationId)
        => Console.WriteLine($"  [Inventory]    released {reservationId}");
}

internal interface IPaymentService
{
    bool Charge(Customer customer, decimal amount);
}

internal class PaymentService : IPaymentService
{
    private readonly bool _simulateFailure;
    public PaymentService(bool simulateFailure = false) => _simulateFailure = simulateFailure;

    public bool Charge(Customer customer, decimal amount)
    {
        if (_simulateFailure)
        {
            Console.WriteLine($"  [Payment]      DECLINED for {customer.Name}");
            return false;
        }
        Console.WriteLine($"  [Payment]      charged {customer.Name} ${amount}");
        return true;
    }
}

internal interface IShippingService
{
    string CreateShipment(Customer customer, Item item);
}

internal class ShippingService : IShippingService
{
    public string CreateShipment(Customer customer, Item item)
    {
        var tracking = $"TRK-{Guid.NewGuid().ToString()[..6]}";
        Console.WriteLine($"  [Shipping]     {item.Sku} -> {customer.Name} ({tracking})");
        return tracking;
    }
}

internal interface INotificationService
{
    void Notify(Customer customer, string trackingNumber);
}

internal class NotificationService : INotificationService
{
    public void Notify(Customer customer, string trackingNumber)
        => Console.WriteLine($"  [Notification] emailed {customer.Email} (tracking {trackingNumber})");
}
