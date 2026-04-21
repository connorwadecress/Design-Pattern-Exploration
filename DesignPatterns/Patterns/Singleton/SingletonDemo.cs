namespace DesignPatterns.Patterns.Singleton;

internal class SingletonDemo : IPatternDemo
{
    public string Name => "Singleton";

    public void Run()
    {
        Console.WriteLine("V1 naive:");
        var a1 = AppLogger.Instance;
        var b1 = AppLogger.Instance;
        a1.Log("from caller A");
        b1.Log("from caller B");
        Console.WriteLine($"  same instance? {ReferenceEquals(a1, b1)}");
        Console.WriteLine();

        Console.WriteLine("V2 thread-safe (double-checked lock):");
        var a2 = AppLoggerThreadSafe.Instance;
        var b2 = AppLoggerThreadSafe.Instance;
        a2.Log("from caller A");
        b2.Log("from caller B");
        Console.WriteLine($"  same instance? {ReferenceEquals(a2, b2)}");
        Console.WriteLine();

        Console.WriteLine("V3 Lazy<T> (idiomatic):");
        var a3 = AppLoggerLazy.Instance;
        var b3 = AppLoggerLazy.Instance;
        a3.Log("from caller A");
        b3.Log("from caller B");
        Console.WriteLine($"  same instance? {ReferenceEquals(a3, b3)}");
    }
}
