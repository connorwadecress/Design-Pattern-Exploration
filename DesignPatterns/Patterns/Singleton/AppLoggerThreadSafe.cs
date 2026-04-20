namespace DesignPatterns.Patterns.Singleton;

/// <summary>
/// Version 2 — thread-safe Singleton using **double-checked locking**.
///
/// Fixes the race condition in v1 by guarding instance creation with a lock.
/// The double-check idiom means we only pay the lock cost on the very first
/// access — once the instance exists, subsequent reads skip locking entirely.
/// </summary>
internal class AppLoggerThreadSafe
{
    private static AppLoggerThreadSafe? _instance;

    // A dedicated, private, readonly lock object. NEVER lock on `this`,
    // on a public type, or on a string — all can be locked by outside code
    // and cause deadlocks. A plain `new object()` that only this class sees
    // is the safe choice.
    private static readonly object _lock = new();

    public Guid InstanceId { get; } = Guid.NewGuid();

    private AppLoggerThreadSafe()
    {
        Console.WriteLine($"  >> AppLoggerThreadSafe constructor ran. InstanceId = {InstanceId}");
    }

    public static AppLoggerThreadSafe Instance
    {
        get
        {
            // (A) Outer check — FAST PATH, no lock.
            //     Once _instance is set, every future read short-circuits here
            //     and avoids the (relatively expensive) lock.
            if (_instance == null)
            {
                lock (_lock)
                {
                    // (B) Inner check — CORRECTNESS.
                    //     While we were waiting for the lock, another thread
                    //     may have already created the instance. If we skipped
                    //     this check we'd create a second one.
                    if (_instance == null)
                    {
                        _instance = new AppLoggerThreadSafe();
                    }
                }
            }
            return _instance;
        }
    }

    public void Log(string message)
    {
        Console.WriteLine($"  [{DateTime.Now:HH:mm:ss}] [{InstanceId}] {message}");
    }
}
