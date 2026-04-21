namespace DesignPatterns.Patterns.State;

/// <summary>
/// TERMINAL state. The order has been delivered.
/// Every operation throws — the order is complete and cannot transition further.
///
/// Terminal states are common in State machines. Modelling them as
/// their own class (rather than as a flag on another state) keeps the
/// rule "no further actions" localised and impossible to miss.
/// </summary>
internal class DeliveredState : IOrderState
{
    public string Name => "Delivered";

    public void Pay(Order order)     => Reject(order, nameof(Pay));
    public void Ship(Order order)    => Reject(order, nameof(Ship));
    public void Deliver(Order order) => Reject(order, nameof(Deliver));
    public void Cancel(Order order)  => Reject(order, nameof(Cancel));

    private static void Reject(Order order, string op)
        => throw new InvalidOperationException(
            $"Order {order.Id}: cannot {op} — order is already delivered (terminal state).");
}
