namespace DesignPatterns.Patterns.Builder;

// Director holds named construction recipes. Useful when the same email shape
// is built from many places. Takes IEmailBuilder, not a concrete type.
internal class EmailDirector
{
    public EmailMessage BuildWelcomeEmail(IEmailBuilder builder, string userEmail, string userName)
    {
        return builder
            .To(userEmail)
            .Subject("Welcome!")
            .Body($"Hi {userName}, thanks for signing up.")
            .Build();
    }
}
