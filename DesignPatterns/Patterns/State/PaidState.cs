namespace DesignPatterns.Patterns.State;

/// <summary>
/// The order has been paid but not yet shipped.
///
/// Legal operations:
///   • Ship    -> transitions to ShippedState.
///   • Cancel  -> transitions to CancelledState (and logs a refund).
///
/// Illegal operations:
///   • Pay     — already paid.
///   • Deliver — not shipped yet.
/// </summary>
internal class PaidState : IOrderState
{
    public string Name => "Paid";

    public void Pay(Order order)
        => throw new InvalidOperationException($"Order {order.Id}: already paid.");

    public void Ship(Order order)
    {
        Console.WriteLine($"    [Paid]            Creating shipment for order {order.Id}...");
        order.SetState(new ShippedState());
    }

    public void Deliver(Order order)
        => throw new InvalidOperationException($"Order {order.Id}: cannot deliver — not shipped yet.");

    public void Cancel(Order order)
    {
        // A cancellation from the Paid state carries a refund — that
        // rule lives here because it's a property of *this* transition,
        // not of the Order class as a whole.
        Console.WriteLine($"    [Paid]            Cancelling order {order.Id}. Issuing refund.");
        order.SetState(new CancelledState());
    }
}
