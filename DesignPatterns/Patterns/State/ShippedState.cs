namespace DesignPatterns.Patterns.State;

/// <summary>
/// The order has been shipped and is on its way to the customer.
///
/// Legal operations:
///   • Deliver -> transitions to DeliveredState.
///
/// Illegal operations:
///   • Pay     — already paid.
///   • Ship    — already shipped.
///   • Cancel  — too late to cancel; the parcel is out for delivery.
/// </summary>
internal class ShippedState : IOrderState
{
    public string Name => "Shipped";

    public void Pay(Order order)
        => throw new InvalidOperationException($"Order {order.Id}: already paid.");

    public void Ship(Order order)
        => throw new InvalidOperationException($"Order {order.Id}: already shipped.");

    public void Deliver(Order order)
    {
        Console.WriteLine($"    [Shipped]         Package delivered for order {order.Id}.");
        order.SetState(new DeliveredState());
    }

    public void Cancel(Order order)
        => throw new InvalidOperationException($"Order {order.Id}: cannot cancel — already shipped.");
}
