namespace DesignPatterns.Patterns.State;

/// <summary>
/// The STATE interface.
///
/// Declares every method whose behaviour varies with the order's state.
/// Each concrete state implements all of these — either doing the work
/// and transitioning the order to a new state, or rejecting the
/// operation with an InvalidOperationException.
///
/// Every method takes the Order (the CONTEXT) as a parameter so the
/// state can call back into the order to transition it. Compare this
/// with Strategy, where strategies typically do NOT know about the
/// context — a key conceptual difference between the two patterns.
/// </summary>
internal interface IOrderState
{
    /// <summary>
    /// Display name for the current state (e.g. "PendingPayment", "Paid").
    /// Used in demo output so you can see transitions happen.
    /// </summary>
    string Name { get; }

    void Pay(Order order);
    void Ship(Order order);
    void Deliver(Order order);
    void Cancel(Order order);
}
