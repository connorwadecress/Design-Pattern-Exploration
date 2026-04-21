namespace DesignPatterns.Patterns.Builder;

internal class BuilderDemo : IPatternDemo
{
    public string Name => "Builder";

    public void Run()
    {
        // Fluent builder used directly.
        Console.WriteLine("Fluent builder:");
        var email = new EmailBuilder()
            .To("alice@example.com")
            .Subject("Lunch?")
            .Body("Free Thursday?")
            .Cc("bob@example.com")
            .Build();
        Console.WriteLine($"  {email}");
        Console.WriteLine();

        // Director using the builder to produce a pre-canned recipe.
        Console.WriteLine("Via director:");
        var welcome = new EmailDirector().BuildWelcomeEmail(new EmailBuilder(), "new@user.com", "Connor");
        Console.WriteLine($"  {welcome}");
        Console.WriteLine();

        // Validation kicks in when a required field is missing.
        Console.WriteLine("Missing required field:");
        try
        {
            new EmailBuilder().To("x@y.com").Subject("oops").Build();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  caught: {ex.Message}");
        }
    }
}
