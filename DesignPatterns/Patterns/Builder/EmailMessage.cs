namespace DesignPatterns.Patterns.Builder;

/// <summary>
/// Priority level for an email. Default when nothing is set is Normal.
/// </summary>
internal enum EmailPriority
{
    Low,
    Normal,
    High
}

/// <summary>
/// A file attachment. Using a record gives us value-equality and a tidy
/// constructor for free — a small demo of modern C# niceties.
/// </summary>
internal record Attachment(string FileName, int SizeBytes);

/// <summary>
/// The PRODUCT of the Builder pattern.
///
/// Immutable on purpose:
///   • All properties are { get; } with no setters.
///   • The constructor is `internal`, so only code in this assembly
///     (specifically the builder) can construct one.
///   • External callers MUST go through <see cref="EmailBuilder"/> —
///     that is how we get step-by-step construction AND validation.
///
/// The lists are exposed as IReadOnlyList to prevent callers mutating
/// the internal collections after the object is built.
/// </summary>
internal class EmailMessage
{
    public string To { get; }
    public string Subject { get; }
    public string Body { get; }
    public IReadOnlyList<string> Cc { get; }
    public IReadOnlyList<string> Bcc { get; }
    public EmailPriority Priority { get; }
    public IReadOnlyList<Attachment> Attachments { get; }

    // internal, not public — the builder is the only legitimate caller.
    internal EmailMessage(
        string to,
        string subject,
        string body,
        IReadOnlyList<string> cc,
        IReadOnlyList<string> bcc,
        EmailPriority priority,
        IReadOnlyList<Attachment> attachments)
    {
        To = to;
        Subject = subject;
        Body = body;
        Cc = cc;
        Bcc = bcc;
        Priority = priority;
        Attachments = attachments;
    }

    /// <summary>
    /// Pretty-prints the message for the demo output.
    /// </summary>
    public string Describe()
    {
        var cc = Cc.Count == 0 ? "(none)" : string.Join(", ", Cc);
        var bcc = Bcc.Count == 0 ? "(none)" : string.Join(", ", Bcc);
        var attach = Attachments.Count == 0
            ? "(none)"
            : string.Join(", ", Attachments.Select(a => $"{a.FileName} [{a.SizeBytes}B]"));

        return $"""
                  To:          {To}
                  Subject:     {Subject}
                  Body:        {Body}
                  Cc:          {cc}
                  Bcc:         {bcc}
                  Priority:    {Priority}
                  Attachments: {attach}
                """;
    }
}
