namespace DesignPatterns.Patterns.Proxy;

// The interface the client speaks. Real and proxy both implement this.
internal interface IDocumentService
{
    string GetDocument(string id, string user);
}

// The "real" expensive subject - real implementation would hit a database
// or pull a file off disk
// this will just return fake body
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
