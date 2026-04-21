namespace DesignPatterns.Patterns.State;

/// <summary>
/// The CONTEXT.
///
/// Holds a reference to the current state and forwards every public
/// operation to that state. Contrast with the naive version of Order
/// (see commentary in StateDemo) where every method was a giant switch
/// on an OrderStatus enum.
///
/// Key design points:
///   • The context's methods are ONE-LINERS that delegate to the state.
///   • The context exposes SetState so states can transition the order.
///   • The context knows nothing about which transitions are legal —
///     that knowledge lives inside each state. Adding a new state
///     doesn't touch this class.
///
/// Note on SetState visibility:
///   It's public because states need to call it. In a larger codebase
///   you'd either keep it `internal` (this project's default since
///   everything lives in one assembly), or expose it only via an
///   `IOrderStateContext` interface that states see instead of the full
///   Order. For a teaching demo this is clean enough.
/// </summary>
internal class Order
{
    private IOrderState _state;

    public string Id { get; }

    public Order(string id)
    {
        Id = id;
        // Every new order starts in PendingPayment. The initial state is
        // itself a design decision — you're declaring the entry point of
        // the state machine.
        _state = new PendingPaymentState();
    }

    /// <summary>
    /// Current state name — useful for display and tests.
    /// </summary>
    public string CurrentStateName => _state.Name;

    /// <summary>
    /// Transition to a new state. Called by the states themselves.
    /// This is the ONE PLACE every transition flows through, which is
    /// what lets us log "from -> to" transitions uniformly.
    /// </summary>
    public void SetState(IOrderState newState)
    {
        var from = _state.Name;
        _state = newState;
        Console.WriteLine($"    [Order {Id}]       Transition: {from} -> {_state.Name}");
    }

    // --- The public API — each method is a one-line delegation. ---
    // If we added a new state (say, AwaitingReturn), NONE of these
    // methods would change. That's Open/Closed in action.

    public void Pay()     => _state.Pay(this);
    public void Ship()    => _state.Ship(this);
    public void Deliver() => _state.Deliver(this);
    public void Cancel()  => _state.Cancel(this);
}
