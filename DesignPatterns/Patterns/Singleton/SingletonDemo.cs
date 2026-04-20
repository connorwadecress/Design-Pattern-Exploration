namespace DesignPatterns.Patterns.Singleton;

/// <summary>
/// Demonstrates the Singleton pattern using <see cref="AppLogger"/>.
///
/// Talking points for the interview:
///  • Private constructor — nobody can do `new AppLogger()`.
///  • Static Instance property — the single access point.
///  • Proof it works: every call to Instance returns the SAME object
///    (same reference, same InstanceId GUID).
///  • This naive version is NOT thread-safe (see v2 and v3).
/// </summary>
internal class SingletonDemo : IPatternDemo
{
    public string Name => "Singleton (v1 — naive)";

    public void Run()
    {
        Console.WriteLine("Step 1: First call to AppLogger.Instance (constructor should run).");
        var loggerA = AppLogger.Instance;
        loggerA.Log("Hello from location A.");
        Console.WriteLine();

        Console.WriteLine("Step 2: Second call to AppLogger.Instance (constructor should NOT run).");
        var loggerB = AppLogger.Instance;
        loggerB.Log("Hello from location B.");
        Console.WriteLine();

        Console.WriteLine("Step 3: Prove they're the same object.");
        Console.WriteLine($"  ReferenceEquals(loggerA, loggerB)  = {ReferenceEquals(loggerA, loggerB)}");
        Console.WriteLine($"  loggerA.InstanceId == loggerB.Id?  = {loggerA.InstanceId == loggerB.InstanceId}");
        Console.WriteLine();

        Console.WriteLine("Observation:");
        Console.WriteLine("  • The constructor message printed only ONCE — the second call reused the existing instance.");
        Console.WriteLine("  • Both log lines carry the same GUID — global single point of access is working.");
    }
}
