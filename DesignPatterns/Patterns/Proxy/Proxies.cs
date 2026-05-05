namespace DesignPatterns.Patterns.Proxy;

// VIRTUAL PROXY - delays creation of the real service until first call.
// saves resources for when subject is expensive
internal class LazyDocumentProxy : IDocumentService
{
    private RealDocumentService? _real;

    public string GetDocument(string id, string user)
    {
        // Real subject is built only when someone actually asks for a document.
        _real ??= new RealDocumentService();
        return _real.GetDocument(id, user);
    }
}

// PROTECTION PROXY - same interface, but blocks unauthorised callers.
// Real service never sees the call when auth fails.
internal class ProtectionDocumentProxy : IDocumentService
{
    private readonly IDocumentService _inner;
    private readonly HashSet<string> _admins;

    public ProtectionDocumentProxy(IDocumentService inner, IEnumerable<string> admins)
    {
        _inner = inner;
        _admins = new HashSet<string>(admins);
    }

    public string GetDocument(string id, string user)
    {
        if (id.StartsWith("classified-") && !_admins.Contains(user))
        {
            Console.WriteLine($"  [Auth]    DENIED {user} -> {id}");
            throw new UnauthorizedAccessException($"{user} cannot read {id}");
        }
        Console.WriteLine($"  [Auth]    allowed {user} -> {id}");
        return _inner.GetDocument(id, user);
    }
}

// CACHING PROXY - returns a Cached result on a hit, only delegates on a miss.
// Real subject's GetDocument runs at most once per document id.
internal class CachingDocumentProxy : IDocumentService
{
    private readonly IDocumentService _inner;
    private readonly Dictionary<string, string> _cache = new();

    public CachingDocumentProxy(IDocumentService inner) => _inner = inner;

    public string GetDocument(string id, string user)
    {
        if (_cache.TryGetValue(id, out var hit))
        {
            Console.WriteLine($"  [Cache]   HIT  {id}");
            return hit;
        }
        Console.WriteLine($"  [Cache]   MISS {id}");
        var result = _inner.GetDocument(id, user);
        _cache[id] = result;
        return result;
    }
}
