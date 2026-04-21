namespace DesignPatterns.Patterns.State;

internal class StateDemo : IPatternDemo
{
    public string Name => "State";

    public void Run()
    {
        Console.WriteLine("Happy path (Pay -> Ship -> Deliver):");
        var order = new Order("ORD-01");
        order.Pay();
        order.Ship();
        order.Deliver();
        Console.WriteLine($"  final state: {order.CurrentStateName}");
        Console.WriteLine();

        Console.WriteLine("Illegal op - try to ship before paying:");
        var order2 = new Order("ORD-02");
        try
        {
            order2.Ship();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  caught: {ex.Message}");
        }
        Console.WriteLine($"  state still: {order2.CurrentStateName}");
    }
}
