namespace DesignPatterns.Patterns.Proxy;

// The interface the client speaks. Real and proxy both implement this.
internal interface IDocumentService
{
    string GetDocument(string id, string user);
}

// The "real" expensive subject. In real life this would hit a database
// or pull a file off disk. We just print and return a fake body.
internal class RealDocumentService : IDocumentService
{
    public RealDocumentService()
    {
        Console.WriteLine("  [Real]    constructed (expensive)");
    }

    public string GetDocument(string id, string user)
    {
        Console.WriteLine($"  [Real]    loading {id} from disk...");
        return $"<contents of {id}>";
    }
}
