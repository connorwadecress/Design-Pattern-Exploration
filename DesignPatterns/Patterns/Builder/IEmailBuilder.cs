namespace DesignPatterns.Patterns.Builder;

/// <summary>
/// Contract for anything that can build an <see cref="EmailMessage"/> step-by-step.
///
/// Every mutator returns <see cref="IEmailBuilder"/> so callers can either:
///   • chain calls fluently (new EmailBuilder().To(...).Subject(...).Build()), OR
///   • ignore the return value and call each method on its own line,
///     which is what the Director does.
///
/// Separating this interface from the concrete builder means:
///   • The Director depends on the abstraction, not on EmailBuilder (DIP).
///   • We could add a SecondEmailBuilder (e.g. one that builds HTML emails
///     with a different internal representation) without changing the Director.
/// </summary>
internal interface IEmailBuilder
{
    IEmailBuilder To(string address);
    IEmailBuilder Subject(string subject);
    IEmailBuilder Body(string body);
    IEmailBuilder Cc(string address);
    IEmailBuilder Bcc(string address);
    IEmailBuilder WithPriority(EmailPriority priority);
    IEmailBuilder AddAttachment(Attachment attachment);

    /// <summary>
    /// Validate the accumulated state and produce the finished product.
    /// Throws <see cref="InvalidOperationException"/> if required fields are missing.
    /// </summary>
    EmailMessage Build();
}
