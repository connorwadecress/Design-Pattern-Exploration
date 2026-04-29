using System.Reflection;

namespace DesignPatterns.Reflection;

// Uses reflection to inspect a Calculator and invoke its methods by NAME
// no hardcodes in this class
// everything is discovered at runtime from the Calculator type itself

// This is the same pattern a DI container uses, just with CONSTRUCTORS
// instead of METHODS: inspect the type, then invoke.
internal class ReflectionCalculator
{
    private readonly Calculator _target = new();

    // Discovery: ask the type what public instance methods it declares.
    public IEnumerable<string> ListOperations()
    {
        return _target.GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Select(m => m.Name);
    }

    // Invocation: find the method by name and call it dynamically.
    public double Execute(string operationName, double a, double b)
    {
        var method = _target.GetType()
            .GetMethod(operationName, BindingFlags.Public | BindingFlags.Instance);

        if (method == null)
            throw new InvalidOperationException($"No operation named '{operationName}'.");

        // Invoke returns an object; we know our calculator methods return double.
        var result = method.Invoke(_target, new object[] { a, b });
        return (double)result!;
    }
}
