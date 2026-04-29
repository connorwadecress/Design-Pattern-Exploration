namespace DesignPatterns.Patterns.Builder;

// The PRODUCT. Immutable - all properties are get-only
//internal so that construction happens through the builder
//cant bypass the builder
internal class EmailMessage
{
    public string To { get; } //get only so its immutable
    public string Subject { get; }
    public string Body { get; }
    public IReadOnlyList<string> Cc { get; }

    internal EmailMessage(string to, string subject, string body, IReadOnlyList<string> cc)
    {
        To = to;
        Subject = subject;
        Body = body;
        Cc = cc;
    }

    public override string ToString()
        => $"To: {To} | Subject: {Subject} | Cc: [{string.Join(", ", Cc)}] | Body: {Body}";
}
