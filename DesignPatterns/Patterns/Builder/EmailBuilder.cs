namespace DesignPatterns.Patterns.Builder;

// Interface makes the director depend on the abstraction, not the concrete builder.
internal interface IEmailBuilder
{
    IEmailBuilder To(string address);
    IEmailBuilder Subject(string subject);
    IEmailBuilder Body(string body);
    IEmailBuilder Cc(string address);
    EmailMessage Build();
}

// Fluent builder. Each method stores a value and returns `this` so calls can be chained.
internal class EmailBuilder : IEmailBuilder
{
    private string? _to;
    private string? _subject;
    private string? _body;
    private readonly List<string> _cc = new();  //copy list in Build() so that already built product can never be mutated by future changes

    public IEmailBuilder To(string address) { _to = address; return this; }
    public IEmailBuilder Subject(string subject) { _subject = subject; return this; }
    public IEmailBuilder Body(string body) { _body = body; return this; }
    public IEmailBuilder Cc(string address) { _cc.Add(address); return this; }

    public EmailMessage Build()
    {
        if (string.IsNullOrWhiteSpace(_to)) throw new InvalidOperationException("To is required.");
        if (string.IsNullOrWhiteSpace(_subject)) throw new InvalidOperationException("Subject is required.");
        if (string.IsNullOrWhiteSpace(_body)) throw new InvalidOperationException("Body is required.");

        // Snapshot the list so later changes don't leak into the built product.
        return new EmailMessage(_to, _subject, _body, _cc.ToList());
    }
}
