namespace DesignPatterns.Patterns.State;

// State interface - every method the Order exposes has a matching method here.
// Each method takes the Order so the state can call back to transition it.
internal interface IOrderState
{
    string Name { get; }
    void Pay(Order order);
    void Ship(Order order);
    void Deliver(Order order);
}

// Starting state. Pay transitions to Paid; Ship and Deliver are illegal.
internal class PendingPaymentState : IOrderState
{
    public string Name => "PendingPayment";

    public void Pay(Order order)
    {
        Console.WriteLine($"  charging for {order.Id}");
        order.SetState(new PaidState());
    }

    public void Ship(Order order) => throw new InvalidOperationException("Can't ship - not paid.");
    public void Deliver(Order order) => throw new InvalidOperationException("Can't deliver - not paid.");
}

// Paid. Ship transitions to Shipped; Pay and Deliver are illegal.
internal class PaidState : IOrderState
{
    public string Name => "Paid";

    public void Pay(Order order) => throw new InvalidOperationException("Already paid.");

    public void Ship(Order order)
    {
        Console.WriteLine($"  shipping {order.Id}");
        order.SetState(new ShippedState());
    }

    public void Deliver(Order order) => throw new InvalidOperationException("Can't deliver - not shipped.");
}

// Shipped. Deliver transitions to Delivered; rest are illegal.
internal class ShippedState : IOrderState
{
    public string Name => "Shipped";

    public void Pay(Order order) => throw new InvalidOperationException("Already paid.");
    public void Ship(Order order) => throw new InvalidOperationException("Already shipped.");

    public void Deliver(Order order)
    {
        Console.WriteLine($"  delivered {order.Id}");
        order.SetState(new DeliveredState());
    }
}

// Terminal state - every operation throws.
internal class DeliveredState : IOrderState
{
    public string Name => "Delivered";

    public void Pay(Order order) => throw new InvalidOperationException("Order is complete.");
    public void Ship(Order order) => throw new InvalidOperationException("Order is complete.");
    public void Deliver(Order order) => throw new InvalidOperationException("Order is complete.");
}
