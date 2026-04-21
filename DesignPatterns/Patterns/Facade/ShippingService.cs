namespace DesignPatterns.Patterns.Facade;

/// <summary>
/// Subsystem service — responsible for creating shipments and returning
/// tracking numbers. Real implementation would talk to a carrier API.
/// </summary>
internal interface IShippingService
{
    string CreateShipment(Customer customer, Item item);
}

internal class ShippingService : IShippingService
{
    public string CreateShipment(Customer customer, Item item)
    {
        var tracking = $"TRK-{Guid.NewGuid().ToString()[..8]}";
        Console.WriteLine($"    [Shipping]      Created shipment of {item.Sku} to {customer.Address}.");
        Console.WriteLine($"    [Shipping]      Tracking number: {tracking}.");
        return tracking;
    }
}
