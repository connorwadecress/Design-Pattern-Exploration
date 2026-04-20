namespace DesignPatterns.Patterns.Singleton;

/// <summary>
/// Version 3 — idiomatic C# Singleton using <see cref="Lazy{T}"/>.
///
/// Lazy&lt;T&gt; is a framework type whose entire job is:
///   "compute this value the first time it's asked for, thread-safely,
///    then cache it forever."
/// …which is *exactly* the Singleton contract. So we let the BCL do the work:
/// no manual null checks, no lock objects, no double-checked dance.
///
/// This is the version you'd reach for in real production C# code
/// (if you needed a hand-rolled Singleton at all — in most modern apps
/// you'd just register it with the DI container instead).
/// </summary>
internal class AppLoggerLazy
{
    // The Lazy<T> wraps the "create it once, safely" logic.
    //
    // The default thread-safety mode is LazyThreadSafetyMode.ExecutionAndPublication,
    // which guarantees the factory lambda runs AT MOST ONCE across all threads —
    // even if a dozen threads hit .Value simultaneously on first access.
    private static readonly Lazy<AppLoggerLazy> _lazy =
        new(() => new AppLoggerLazy());

    public Guid InstanceId { get; } = Guid.NewGuid();

    // Private constructor — same reason as v1 and v2: nobody outside may `new` us.
    // The Lazy<T> factory above is inside the class so it CAN call it.
    private AppLoggerLazy()
    {
        Console.WriteLine($"  >> AppLoggerLazy constructor ran. InstanceId = {InstanceId}");
    }

    // Expression-bodied property — compact shorthand for { get { return _lazy.Value; } }.
    public static AppLoggerLazy Instance => _lazy.Value;

    public void Log(string message)
    {
        Console.WriteLine($"  [{DateTime.Now:HH:mm:ss}] [{InstanceId}] {message}");
    }
}
