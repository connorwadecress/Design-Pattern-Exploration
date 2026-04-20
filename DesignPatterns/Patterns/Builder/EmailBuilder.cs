namespace DesignPatterns.Patterns.Builder;

/// <summary>
/// Concrete fluent builder for <see cref="EmailMessage"/>.
///
/// Collects values as the caller walks through the mutator methods, then
/// validates and constructs the immutable product when <see cref="Build"/> is called.
///
/// The mutators return `IEmailBuilder` (this) so calls can be chained.
/// The internal collections (`_cc`, `_bcc`, `_attachments`) are mutated in place
/// here because the builder itself is mutable — the IMMUTABILITY is only
/// required on the product, not on the thing building it.
/// </summary>
internal class EmailBuilder : IEmailBuilder
{
    // Required fields — start null, must be set before Build().
    private string? _to;
    private string? _subject;
    private string? _body;

    // Optional collections — start empty.
    private readonly List<string> _cc = new();
    private readonly List<string> _bcc = new();
    private readonly List<Attachment> _attachments = new();

    // Optional with a sensible default.
    private EmailPriority _priority = EmailPriority.Normal;

    // --- Mutators ---
    // Each one stores a value (or appends to a list) and returns `this`
    // so the caller can chain another method.

    public IEmailBuilder To(string address)
    {
        _to = address;
        return this;
    }

    public IEmailBuilder Subject(string subject)
    {
        _subject = subject;
        return this;
    }

    public IEmailBuilder Body(string body)
    {
        _body = body;
        return this;
    }

    public IEmailBuilder Cc(string address)
    {
        _cc.Add(address);
        return this;
    }

    public IEmailBuilder Bcc(string address)
    {
        _bcc.Add(address);
        return this;
    }

    public IEmailBuilder WithPriority(EmailPriority priority)
    {
        _priority = priority;
        return this;
    }

    public IEmailBuilder AddAttachment(Attachment attachment)
    {
        _attachments.Add(attachment);
        return this;
    }

    // --- The commit point ---

    public EmailMessage Build()
    {
        // Validation: enforce required fields. If any is missing, construction fails
        // cleanly rather than producing a half-formed EmailMessage.
        if (string.IsNullOrWhiteSpace(_to))
            throw new InvalidOperationException("Email requires a 'To' address.");
        if (string.IsNullOrWhiteSpace(_subject))
            throw new InvalidOperationException("Email requires a subject.");
        if (string.IsNullOrWhiteSpace(_body))
            throw new InvalidOperationException("Email requires a body.");

        // Snapshot the collections with ToList() so that any further changes the
        // builder receives after Build() don't leak into the already-built object.
        // This keeps the product genuinely immutable.
        return new EmailMessage(
            _to,
            _subject,
            _body,
            _cc.ToList(),
            _bcc.ToList(),
            _priority,
            _attachments.ToList());
    }
}
