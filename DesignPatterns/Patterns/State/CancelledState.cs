namespace DesignPatterns.Patterns.State;

/// <summary>
/// TERMINAL state. The order has been cancelled.
/// Every operation throws — a cancelled order cannot transition further.
/// </summary>
internal class CancelledState : IOrderState
{
    public string Name => "Cancelled";

    public void Pay(Order order)     => Reject(order, nameof(Pay));
    public void Ship(Order order)    => Reject(order, nameof(Ship));
    public void Deliver(Order order) => Reject(order, nameof(Deliver));
    public void Cancel(Order order)  => Reject(order, nameof(Cancel));

    private static void Reject(Order order, string op)
        => throw new InvalidOperationException(
            $"Order {order.Id}: cannot {op} — order is already cancelled (terminal state).");
}
