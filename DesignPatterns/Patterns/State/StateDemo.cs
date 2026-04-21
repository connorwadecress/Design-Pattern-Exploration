namespace DesignPatterns.Patterns.State;

/// <summary>
/// Demonstrates the State pattern via an order lifecycle:
///   PendingPayment -> Paid -> Shipped -> Delivered
///                        \-> Cancelled (from Paid with refund)
///   PendingPayment -> Cancelled (no refund)
///
/// Four scenarios:
///   1. Happy path: Pay -> Ship -> Deliver.
///   2. Cancel from Paid (with refund).
///   3. Illegal op: try to Ship an unpaid order — caught, printed.
///   4. Illegal op on terminal state: cancel a delivered order — caught.
///
/// Interview talking points:
///   • The Order (context) has ZERO branching on status. The naive version
///     would have a giant switch inside every method; here each method is
///     a one-line delegation.
///   • Each state class is small, focused, and independently testable.
///   • Transitions live in the state they transition FROM — no central
///     switch statement orchestrating the lifecycle.
///   • Adding a new state (e.g. AwaitingReturn) = new class + transitions
///     added to the states that should transition into it. The Order
///     class is untouched. Open/Closed Principle.
///   • Strategy vs State: structurally identical (interface + concrete
///     implementations + context), but Strategy is picked by the CLIENT
///     and rarely changes, while State is picked by the CONTEXT itself
///     in response to events and changes frequently. Strategy = algorithm
///     family; State = modes of being.
/// </summary>
internal class StateDemo : IPatternDemo
{
    public string Name => "State (order lifecycle)";

    public void Run()
    {
        RunHappyPath();
        Separator();
        RunCancelFromPaid();
        Separator();
        RunIllegalOperation();
        Separator();
        RunOperationOnTerminalState();
        Separator();
        PrintSummary();
    }

    private static void RunHappyPath()
    {
        Console.WriteLine("=== Scenario 1: Happy path (Pay -> Ship -> Deliver) ===");
        Console.WriteLine();

        var order = new Order("ORD-01");
        ShowState(order);

        order.Pay();        ShowState(order);
        order.Ship();       ShowState(order);
        order.Deliver();    ShowState(order);
    }

    private static void RunCancelFromPaid()
    {
        Console.WriteLine("=== Scenario 2: Cancel from Paid (triggers refund) ===");
        Console.WriteLine("Notice the refund is logged by PaidState, not by Order.");
        Console.WriteLine("The rule \"cancelling a paid order triggers a refund\" lives in");
        Console.WriteLine("the state where that transition originates.");
        Console.WriteLine();

        var order = new Order("ORD-02");
        ShowState(order);

        order.Pay();        ShowState(order);
        order.Cancel();     ShowState(order);
    }

    private static void RunIllegalOperation()
    {
        Console.WriteLine("=== Scenario 3: Illegal operation (ship an unpaid order) ===");
        Console.WriteLine("The state itself enforces the rule — Order.Ship has no branching.");
        Console.WriteLine();

        var order = new Order("ORD-03");
        ShowState(order);

        try
        {
            order.Ship();
            Console.WriteLine("  (we should never reach this line)");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  Caught expected exception: \"{ex.Message}\"");
        }

        ShowState(order); // still PendingPayment — the failed op did not mutate state
    }

    private static void RunOperationOnTerminalState()
    {
        Console.WriteLine("=== Scenario 4: Any operation on a terminal state ===");
        Console.WriteLine("Delivered is terminal. Every method on DeliveredState throws.");
        Console.WriteLine();

        var order = new Order("ORD-04");
        order.Pay();
        order.Ship();
        order.Deliver();
        ShowState(order);

        try
        {
            order.Cancel();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  Caught expected exception: \"{ex.Message}\"");
        }
    }

    private static void ShowState(Order order)
    {
        Console.WriteLine($"  Current state of {order.Id}: {order.CurrentStateName}");
        Console.WriteLine();
    }

    private static void PrintSummary()
    {
        Console.WriteLine("=== Summary ===");
        Console.WriteLine("  State replaces per-method branching on a status enum with a");
        Console.WriteLine("  class-per-state design. Each state handles every method, either");
        Console.WriteLine("  doing the work (and transitioning) or rejecting the operation.");
        Console.WriteLine();
        Console.WriteLine("  Order class (the CONTEXT) has no `switch(_status)` anywhere —");
        Console.WriteLine("  every public method is a one-line delegation to _state. Adding");
        Console.WriteLine("  a new state doesn't touch Order at all. Open/Closed Principle.");
        Console.WriteLine();
        Console.WriteLine("  Trade-offs:");
        Console.WriteLine("    • More classes than a plain enum + switch (one per state).");
        Console.WriteLine("    • States are coupled to each other (PaidState knows ShippedState).");
        Console.WriteLine("    • The full state machine is only visible by reading every state.");
        Console.WriteLine();
        Console.WriteLine("  State vs Strategy — the classic interview comparison:");
        Console.WriteLine("    Strategy -> CLIENT picks the algorithm. Rarely changes. Algorithms");
        Console.WriteLine("                are independent (don't know about each other).");
        Console.WriteLine("    State    -> CONTEXT picks the next state ITSELF. Changes often.");
        Console.WriteLine("                States know about each other (they construct the next).");
    }

    private static void Separator()
    {
        Console.WriteLine();
        Console.WriteLine(new string('-', 72));
        Console.WriteLine();
    }
}
