namespace DesignPatterns.Patterns.State;

// The context. Holds the current state and forwards every public method to it.
// Notice the complete absence of any if/switch on status - that's the payoff.
internal class Order
{
    private IOrderState _state;

    public string Id { get; }
    public string CurrentStateName => _state.Name;

    public Order(string id)
    {
        Id = id;
        _state = new PendingPaymentState();
    }

    public void SetState(IOrderState newState)
    {
        Console.WriteLine($"  {Id}: {_state.Name} -> {newState.Name}");
        _state = newState;
    }

    public void Pay() => _state.Pay(this);
    public void Ship() => _state.Ship(this);
    public void Deliver() => _state.Deliver(this);
}
