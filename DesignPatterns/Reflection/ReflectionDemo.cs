namespace DesignPatterns.Reflection;

internal class ReflectionDemo : IPatternDemo
{
    public string Name => "Reflection (Calculator)";

    public void Run()
    {
        var calc = new ReflectionCalculator();

        // Discovery - reflection tells us what methods exist at runtime.
        Console.WriteLine("Operations discovered via reflection:");
        foreach (var op in calc.ListOperations())
            Console.WriteLine($"  - {op}");
        Console.WriteLine();

        // Invocation - reflection calls those methods by name.
        Console.WriteLine("Invoking by name:");
        Console.WriteLine($"  Add(5, 3)       = {calc.Execute("Add", 5, 3)}");
        Console.WriteLine($"  Subtract(10, 4) = {calc.Execute("Subtract", 10, 4)}");
        Console.WriteLine($"  Multiply(6, 7)  = {calc.Execute("Multiply", 6, 7)}");
        Console.WriteLine($"  Power(2, 8)     = {calc.Execute("Power", 2, 8)}");
        Console.WriteLine();

        // What happens when the name doesn't exist.
        Console.WriteLine("Unknown operation:");
        try
        {
            calc.Execute("Sqrt", 16, 0);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  caught: {ex.Message}");
        }
        Console.WriteLine();

        // The DI connection - the point of this demo.
        Console.WriteLine("Why DI containers use reflection:");
        Console.WriteLine("  A DI container does the same thing - but with CONSTRUCTORS, not methods.");
        Console.WriteLine("  When you register services.AddTransient<IFoo, Foo>(), the container has");
        Console.WriteLine("  no compile-time knowledge of Foo's dependencies. When you resolve IFoo:");
        Console.WriteLine("    1. It finds the Foo type.");
        Console.WriteLine("    2. It inspects Foo's constructor parameters.");
        Console.WriteLine("    3. It recursively resolves each parameter type.");
        Console.WriteLine("    4. It invokes the constructor with the resolved dependencies.");
        Console.WriteLine("  Same inspect-then-invoke pattern we just used - just on constructors.");
    }
}
