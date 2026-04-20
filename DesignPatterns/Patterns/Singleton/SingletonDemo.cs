namespace DesignPatterns.Patterns.Singleton;

/// <summary>
/// Demonstrates three flavours of the Singleton pattern side-by-side:
///   v1 — naive (not thread-safe)
///   v2 — thread-safe via double-checked locking
///   v3 — idiomatic C# via Lazy&lt;T&gt;
///
/// Each section proves the Singleton guarantee by showing:
///   • The constructor runs only ONCE (one ">> constructor ran" line).
///   • Two separate calls to .Instance return the same reference and
///     therefore the same InstanceId GUID.
///
/// Interview talking points (top of mind):
///   • Private constructor is what enforces uniqueness — without it, the
///     pattern is cosmetic only.
///   • Singleton hides dependencies and is hostile to unit tests, which is
///     why it's often called an anti-pattern.
///   • In a DI-based app, prefer services.AddSingleton&lt;T&gt;() — same
///     *lifetime*, but the container owns it, not the class. This keeps
///     dependencies explicit (constructor-injected) and swappable in tests.
/// </summary>
internal class SingletonDemo : IPatternDemo
{
    public string Name => "Singleton (v1 naive, v2 thread-safe, v3 Lazy<T>)";

    public void Run()
    {
        RunV1Naive();
        Separator();
        RunV2ThreadSafe();
        Separator();
        RunV3Lazy();
        Separator();
        PrintSummary();
    }

    private static void RunV1Naive()
    {
        Console.WriteLine("=== V1: Naive Singleton (NOT thread-safe) ===");
        var a = AppLogger.Instance;
        a.Log("Log from caller A.");
        var b = AppLogger.Instance;
        b.Log("Log from caller B.");
        Console.WriteLine($"  ReferenceEquals(a, b)          = {ReferenceEquals(a, b)}");
        Console.WriteLine($"  a.InstanceId == b.InstanceId?  = {a.InstanceId == b.InstanceId}");
        Console.WriteLine("  ⚠ Risk: under concurrent first-access, two threads can both pass");
        Console.WriteLine("    the null check and construct separate instances.");
    }

    private static void RunV2ThreadSafe()
    {
        Console.WriteLine("=== V2: Thread-safe Singleton (double-checked locking) ===");
        var a = AppLoggerThreadSafe.Instance;
        a.Log("Log from caller A.");
        var b = AppLoggerThreadSafe.Instance;
        b.Log("Log from caller B.");
        Console.WriteLine($"  ReferenceEquals(a, b)          = {ReferenceEquals(a, b)}");
        Console.WriteLine($"  a.InstanceId == b.InstanceId?  = {a.InstanceId == b.InstanceId}");
        Console.WriteLine("  ✔ Safe under concurrency — but the code is fiddly and the");
        Console.WriteLine("    double-check idiom is notorious for subtle mistakes.");
    }

    private static void RunV3Lazy()
    {
        Console.WriteLine("=== V3: Lazy<T> Singleton (idiomatic C#) ===");
        var a = AppLoggerLazy.Instance;
        a.Log("Log from caller A.");
        var b = AppLoggerLazy.Instance;
        b.Log("Log from caller B.");
        Console.WriteLine($"  ReferenceEquals(a, b)          = {ReferenceEquals(a, b)}");
        Console.WriteLine($"  a.InstanceId == b.InstanceId?  = {a.InstanceId == b.InstanceId}");
        Console.WriteLine("  ✔ Thread-safe AND concise — Lazy<T> does the work. This is the");
        Console.WriteLine("    modern default if you're writing the Singleton by hand.");
    }

    private static void PrintSummary()
    {
        Console.WriteLine("=== Summary ===");
        Console.WriteLine("  v1 naive    : simplest, clearest teaching example — broken under threads.");
        Console.WriteLine("  v2 lock     : correct, verbose, easy to get the double-check wrong.");
        Console.WriteLine("  v3 Lazy<T>  : correct, concise — the idiomatic C# answer.");
        Console.WriteLine();
        Console.WriteLine("Bigger picture — connecting to DI (the previous topic):");
        Console.WriteLine("  In a real app, prefer:  services.AddSingleton<ILogger, AppLogger>();");
        Console.WriteLine("  Benefits over a hand-rolled Singleton:");
        Console.WriteLine("    • Dependencies are explicit (constructor-injected), not hidden behind .Instance.");
        Console.WriteLine("    • The class focuses on its job; the container owns the lifetime (SRP).");
        Console.WriteLine("    • In tests you can swap the real implementation for a fake — with a");
        Console.WriteLine("      hand-rolled Singleton you're stuck with the one global instance.");
    }

    private static void Separator()
    {
        Console.WriteLine();
        Console.WriteLine(new string('-', 64));
        Console.WriteLine();
    }
}
