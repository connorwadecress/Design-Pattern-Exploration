namespace DesignPatterns.Patterns.AbstractFactory;

/// <summary>
/// Demonstrates Abstract Factory by rendering the same LoginScreen
/// under two different themes. The LoginScreen code never changes —
/// only the factory passed in changes. That's the entire point.
///
/// Interview talking points:
///   • Abstract Factory produces FAMILIES of related objects.
///   • The client depends on the factory INTERFACE, not on concrete types.
///   • Adding a new THEME = one new factory + N new products. The client
///     and all existing code are untouched (Open/Closed Principle).
///   • Difference from Factory Method: Factory Method has ONE creation
///     method (often overridden by a subclass); Abstract Factory has
///     MANY creation methods on one interface, ensuring family consistency.
///   • DI-container fit: register IUiFactory as a singleton pointing at
///     the concrete factory chosen by configuration. Every class that
///     receives IUiFactory automatically gets the right family.
/// </summary>
internal class AbstractFactoryDemo : IPatternDemo
{
    public string Name => "Abstract Factory (UI theming)";

    public void Run()
    {
        Console.WriteLine("Same client code. Different factory. Watch the UI change.");
        Console.WriteLine();

        RunWithLightTheme();
        Separator();
        RunWithDarkTheme();
        Separator();
        RunWithRuntimeChoice();
        Separator();
        PrintSummary();
    }

    private static void RunWithLightTheme()
    {
        Console.WriteLine("=== LIGHT theme ===");
        IUiFactory factory = new LightThemeFactory();
        var screen = new LoginScreen(factory);
        screen.Render();
    }

    private static void RunWithDarkTheme()
    {
        Console.WriteLine("=== DARK theme ===");
        IUiFactory factory = new DarkThemeFactory();
        var screen = new LoginScreen(factory);
        screen.Render();
    }

    private static void RunWithRuntimeChoice()
    {
        Console.WriteLine("=== Runtime family selection ===");
        Console.WriteLine("A realistic scenario: a config value chooses the family once,");
        Console.WriteLine("and every widget produced from then on is guaranteed to match.");
        Console.WriteLine();

        // Imagine this came from appsettings.json or user preferences.
        var userPrefersDark = true;

        IUiFactory factory = userPrefersDark
            ? new DarkThemeFactory()
            : new LightThemeFactory();

        Console.WriteLine($"  userPrefersDark = {userPrefersDark}");
        Console.WriteLine($"  factory         = {factory.GetType().Name}");
        Console.WriteLine();

        new LoginScreen(factory).Render();
    }

    private static void PrintSummary()
    {
        Console.WriteLine("=== Summary ===");
        Console.WriteLine("  Abstract Factory produces FAMILIES of related objects.");
        Console.WriteLine("  The LoginScreen class above is IDENTICAL in all three runs —");
        Console.WriteLine("  swapping the factory is the only change. That's the payoff:");
        Console.WriteLine("  client code is decoupled from the concrete family.");
        Console.WriteLine();
        Console.WriteLine("  Trade-off:");
        Console.WriteLine("    • Easy to add a new FAMILY      -> new factory + new products.");
        Console.WriteLine("    • Hard  to add a new PRODUCT    -> every factory must implement it.");
        Console.WriteLine();
        Console.WriteLine("  Contrast with Factory Method:");
        Console.WriteLine("    Factory Method -> one overridable creation method for ONE product.");
        Console.WriteLine("    Abstract Factory -> an interface with MANY creation methods for a");
        Console.WriteLine("    family of matching products.");
    }

    private static void Separator()
    {
        Console.WriteLine();
        Console.WriteLine(new string('-', 64));
        Console.WriteLine();
    }
}
