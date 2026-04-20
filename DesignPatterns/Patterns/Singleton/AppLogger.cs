namespace DesignPatterns.Patterns.Singleton;

/// <summary>
/// Version 1 — the classic "textbook" Singleton.
///
/// Guarantees only ONE AppLogger ever exists, accessible globally via
/// AppLogger.Instance.
///
/// ⚠ This version is NOT thread-safe. See v2 and v3 for fixes.
/// </summary>
internal class AppLogger
{
    // (2) Private static field that holds the one-and-only instance.
    //     Nullable (AppLogger?) because it starts out null and is created
    //     lazily on first access.
    private static AppLogger? _instance;

    /// <summary>
    /// A unique id for this instance. If the Singleton is working correctly,
    /// every call to AppLogger.Instance will show the SAME id.
    /// </summary>
    public Guid InstanceId { get; } = Guid.NewGuid();

    // (1) Private constructor — the hallmark of a Singleton.
    //     Stops any outside code from doing `new AppLogger()`.
    //     Only the class itself can construct an instance (inside Instance below).
    private AppLogger()
    {
        Console.WriteLine($"  >> AppLogger constructor ran. InstanceId = {InstanceId}");
    }

    // (3) The global access point.
    //     On first access, create the instance. On every subsequent access,
    //     return the one we already have.
    public static AppLogger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AppLogger();
            }
            return _instance;
        }
    }

    public void Log(string message)
    {
        Console.WriteLine($"  [{DateTime.Now:HH:mm:ss}] [{InstanceId}] {message}");
    }
}
