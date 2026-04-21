namespace DesignPatterns.Patterns.Facade;

/// <summary>
/// Subsystem service — responsible for stock levels and reservations.
/// In a real app this would talk to a database, a warehouse API, etc.
/// </summary>
internal interface IInventoryService
{
    bool CheckStock(Item item);
    string Reserve(Item item);
    void Release(string reservationId);
}

internal class InventoryService : IInventoryService
{
    public bool CheckStock(Item item)
    {
        Console.WriteLine($"    [Inventory]     Checking stock for {item.Sku}...");
        return true; // always in stock for this demo
    }

    public string Reserve(Item item)
    {
        // A short reservation id so the demo output stays readable.
        var id = $"RES-{Guid.NewGuid().ToString()[..8]}";
        Console.WriteLine($"    [Inventory]     Reserved {item.Sku} under {id}.");
        return id;
    }

    public void Release(string reservationId)
    {
        Console.WriteLine($"    [Inventory]     Released reservation {reservationId}.");
    }
}
