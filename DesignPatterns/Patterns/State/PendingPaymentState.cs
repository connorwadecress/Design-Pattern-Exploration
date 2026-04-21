namespace DesignPatterns.Patterns.State;

/// <summary>
/// INITIAL state. The order has been placed but the customer has not paid yet.
///
/// Legal operations:
///   • Pay     -> transitions to PaidState.
///   • Cancel  -> transitions to CancelledState.
///
/// Illegal operations (throw InvalidOperationException):
///   • Ship    — can't ship an unpaid order.
///   • Deliver — can't deliver an unpaid order.
/// </summary>
internal class PendingPaymentState : IOrderState
{
    public string Name => "PendingPayment";

    public void Pay(Order order)
    {
        Console.WriteLine($"    [PendingPayment]  Charging customer for order {order.Id}...");
        // Transition. The state itself decides which state comes next —
        // this is where the state-machine edges live.
        order.SetState(new PaidState());
    }

    public void Ship(Order order)
        => throw new InvalidOperationException($"Order {order.Id}: cannot ship — not paid yet.");

    public void Deliver(Order order)
        => throw new InvalidOperationException($"Order {order.Id}: cannot deliver — not paid yet.");

    public void Cancel(Order order)
    {
        Console.WriteLine($"    [PendingPayment]  Cancelling unpaid order {order.Id}. No refund needed.");
        order.SetState(new CancelledState());
    }
}
