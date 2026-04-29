namespace DesignPatterns.Patterns.Proxy;

internal class ProxyDemo : IPatternDemo
{
    public string Name => "Proxy";

    public void Run()
    {
        // 1. Virtual (lazy) proxy - real service is NOT built up front.
        Console.WriteLine("Virtual proxy - constructed but no real service yet:");
        IDocumentService lazy = new LazyDocumentProxy();
        Console.WriteLine("  ...first call triggers construction:");
        lazy.GetDocument("doc-1", "alice");

        Console.WriteLine();

        // 2. Protection proxy - admin allowed, normal user denied.
        Console.WriteLine("Protection proxy - admin reads classified, user blocked:");
        IDocumentService protectedSvc = new ProtectionDocumentProxy(
            new RealDocumentService(),
            admins: new[] { "alice" });
        protectedSvc.GetDocument("classified-x", "alice");
        try
        {
            protectedSvc.GetDocument("classified-x", "bob");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"  caught: {ex.Message}");
        }

        Console.WriteLine();

        // 3. Caching proxy - second read of the same id never hits the real service.
        Console.WriteLine("Caching proxy - second call is a HIT:");
        IDocumentService cached = new CachingDocumentProxy(new RealDocumentService());
        cached.GetDocument("doc-2", "alice");
        cached.GetDocument("doc-2", "alice");
        cached.GetDocument("doc-3", "alice");

        Console.WriteLine();
        Console.WriteLine("All three proxies share the IDocumentService interface - clients can't tell.");
    }
}
