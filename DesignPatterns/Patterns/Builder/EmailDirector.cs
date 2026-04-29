namespace DesignPatterns.Patterns.Builder;

// Director holds named construction recipes
// Takes IEmailBuilder, not a concrete type.
// knows which steps to call in which order
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
