namespace DesignPatterns.Patterns.Singleton;

// Three flavours of Singleton, same job, progressively better.
// Each uses an InstanceId so you can prove they're the same object across calls.

// V1 - naive. Not thread-safe: two threads could both pass the null check.
internal class AppLogger
{
    private static AppLogger? _instance;

    public Guid InstanceId { get; } = Guid.NewGuid();

    private AppLogger() { }

    public static AppLogger Instance
    {
        get
        {
            if (_instance == null)
                _instance = new AppLogger();
            return _instance;
        }
    }

    public void Log(string message)
        => Console.WriteLine($"  [{InstanceId.ToString()[..8]}] {message}");
}

// V2 - thread-safe with double-checked locking.
// Outer null check = fast path (skip the lock after instance exists).
// Inner null check = correctness (another thread may have created it while we waited).
internal class AppLoggerThreadSafe
{
    private static AppLoggerThreadSafe? _instance;
    private static readonly object _lock = new();

    public Guid InstanceId { get; } = Guid.NewGuid();

    private AppLoggerThreadSafe() { }

    public static AppLoggerThreadSafe Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new AppLoggerThreadSafe();
                }
            }
            return _instance;
        }
    }

    public void Log(string message)
        => Console.WriteLine($"  [{InstanceId.ToString()[..8]}] {message}");
}

// V3 - Lazy<T>. Idiomatic modern C#. Default thread-safety mode guarantees
// the factory runs at most once across all threads.
internal class AppLoggerLazy
{
    private static readonly Lazy<AppLoggerLazy> _lazy = new(() => new AppLoggerLazy());

    public Guid InstanceId { get; } = Guid.NewGuid();

    private AppLoggerLazy() { }

    public static AppLoggerLazy Instance => _lazy.Value;

    public void Log(string message)
        => Console.WriteLine($"  [{InstanceId.ToString()[..8]}] {message}");
}
