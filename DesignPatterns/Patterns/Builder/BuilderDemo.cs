namespace DesignPatterns.Patterns.Builder;

/// <summary>
/// Demonstrates the Builder pattern in three scenarios:
///
///   1. Fluent builder used DIRECTLY — ad-hoc email construction at a call site.
///   2. Fluent builder used via a DIRECTOR — pre-canned "welcome", "password
///      reset", and "invoice" recipes.
///   3. FAILED build — shows validation kicking in when a required field is
///      missing. Demonstrates that the product cannot enter existence in an
///      invalid state.
///
/// Interview talking points:
///   • The PRODUCT (EmailMessage) is immutable — once built, it cannot change.
///   • The BUILDER is mutable — it accumulates state, then produces the product.
///   • The DIRECTOR decouples construction recipes from the builder.
///   • Builder vs Factory: factory = one-call construction; builder = many-step
///     configurable construction with validation at the end.
///   • In modern C#, the FLUENT flavour is far more common than the GoF director
///     flavour (see HostBuilder, StringBuilder, EF Core's model configuration).
/// </summary>
internal class BuilderDemo : IPatternDemo
{
    public string Name => "Builder (fluent + director)";

    public void Run()
    {
        RunFluentDirect();
        Separator();
        RunViaDirector();
        Separator();
        RunValidationFailure();
        Separator();
        PrintSummary();
    }

    private static void RunFluentDirect()
    {
        Console.WriteLine("=== Scenario 1: Fluent builder used directly ===");
        Console.WriteLine("Caller chains method calls to configure the email inline.");
        Console.WriteLine();

        var email = new EmailBuilder()
            .To("alice@example.com")
            .Subject("Lunch?")
            .Body("Free on Thursday?")
            .Cc("bob@example.com")
            .WithPriority(EmailPriority.Low)
            .Build();

        Console.WriteLine(email.Describe());
    }

    private static void RunViaDirector()
    {
        Console.WriteLine("=== Scenario 2: Builder used via the Director ===");
        Console.WriteLine("Director holds named recipes. Caller just picks one.");
        Console.WriteLine();

        var director = new EmailDirector();

        Console.WriteLine("-- Welcome email --");
        var welcome = director.BuildWelcomeEmail(new EmailBuilder(), "newuser@example.com", "Connor");
        Console.WriteLine(welcome.Describe());
        Console.WriteLine();

        Console.WriteLine("-- Password-reset email --");
        var reset = director.BuildPasswordResetEmail(new EmailBuilder(), "newuser@example.com", "https://example.com/reset?t=abc");
        Console.WriteLine(reset.Describe());
        Console.WriteLine();

        Console.WriteLine("-- Invoice email --");
        var invoice = director.BuildInvoiceEmail(
            new EmailBuilder(),
            "customer@example.com",
            "INV-00042",
            new Attachment("invoice-00042.pdf", SizeBytes: 18_432));
        Console.WriteLine(invoice.Describe());
    }

    private static void RunValidationFailure()
    {
        Console.WriteLine("=== Scenario 3: Build() with missing required field ===");
        Console.WriteLine("Proving the product cannot exist in an invalid state.");
        Console.WriteLine();

        try
        {
            // Deliberately forget to set Body.
            var broken = new EmailBuilder()
                .To("someone@example.com")
                .Subject("Oops")
                .Build();

            Console.WriteLine("(We should never reach this line.)");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  Caught expected InvalidOperationException: \"{ex.Message}\"");
            Console.WriteLine("  The builder refused to produce an EmailMessage — exactly the point.");
        }
    }

    private static void PrintSummary()
    {
        Console.WriteLine("=== Summary ===");
        Console.WriteLine("  Builder solves the 'telescoping constructor' problem for objects with many");
        Console.WriteLine("  optional fields. It also lets us validate at a single commit point and return");
        Console.WriteLine("  an immutable product.");
        Console.WriteLine();
        Console.WriteLine("  Fluent flavour — chained method calls, used directly at the call site.");
        Console.WriteLine("  Director flavour — pre-canned named recipes, good when the same construction");
        Console.WriteLine("                     sequence is reused from many places.");
        Console.WriteLine();
        Console.WriteLine("  Relationship to other topics:");
        Console.WriteLine("    • SRP — product's job is to BE the email; builder's job is to CONSTRUCT it.");
        Console.WriteLine("    • DIP — director depends on IEmailBuilder, not EmailBuilder.");
        Console.WriteLine("    • Immutability — product has no setters; once built, cannot change.");
        Console.WriteLine();
        Console.WriteLine("  When NOT to use Builder:");
        Console.WriteLine("    • The object has 2-3 required fields and nothing optional — a constructor");
        Console.WriteLine("      is simpler. Don't over-engineer.");
    }

    private static void Separator()
    {
        Console.WriteLine();
        Console.WriteLine(new string('-', 64));
        Console.WriteLine();
    }
}
