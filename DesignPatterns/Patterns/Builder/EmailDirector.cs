namespace DesignPatterns.Patterns.Builder;

/// <summary>
/// The DIRECTOR in the classic Gang-of-Four Builder pattern.
///
/// The director holds CONSTRUCTION RECIPES — named sequences of build steps
/// that produce specific flavours of the product. A typical app has several
/// directors, or one director with many recipe methods.
///
/// Key points:
///   • The director depends on the ABSTRACTION <see cref="IEmailBuilder"/>,
///     not on the concrete <see cref="EmailBuilder"/>. Any builder will do.
///   • The CALLER supplies the builder. This separates "which kind of
///     builder" (caller's concern) from "what steps to run" (director's concern).
///   • The director never exposes the raw builder — callers get back a finished
///     <see cref="EmailMessage"/>.
///
/// When is a director worth it?
///   • When you have 2+ NAMED recipes that should not live inline at call sites.
///   • When you want construction logic testable in isolation.
///   • Not worth it for one-off ad-hoc emails — just use the fluent builder directly.
/// </summary>
internal class EmailDirector
{
    /// <summary>
    /// Recipe: a simple "welcome" email for new users.
    /// </summary>
    public EmailMessage BuildWelcomeEmail(IEmailBuilder builder, string userEmail, string userName)
    {
        return builder
            .To(userEmail)
            .Subject("Welcome to the service!")
            .Body($"Hi {userName}, thanks for signing up. We're glad to have you.")
            .WithPriority(EmailPriority.Normal)
            .Build();
    }

    /// <summary>
    /// Recipe: a password-reset email. Always high priority, always CCs security.
    /// </summary>
    public EmailMessage BuildPasswordResetEmail(IEmailBuilder builder, string userEmail, string resetLink)
    {
        return builder
            .To(userEmail)
            .Subject("Password reset requested")
            .Body($"A password reset was requested for your account. Click: {resetLink}")
            .Cc("security-audit@example.com")
            .WithPriority(EmailPriority.High)
            .Build();
    }

    /// <summary>
    /// Recipe: a monthly invoice email with a PDF attachment.
    /// </summary>
    public EmailMessage BuildInvoiceEmail(IEmailBuilder builder, string customerEmail, string invoiceNumber, Attachment pdf)
    {
        return builder
            .To(customerEmail)
            .Subject($"Your invoice {invoiceNumber}")
            .Body($"Please find invoice {invoiceNumber} attached. Payment due in 30 days.")
            .AddAttachment(pdf)
            .WithPriority(EmailPriority.Normal)
            .Build();
    }
}
