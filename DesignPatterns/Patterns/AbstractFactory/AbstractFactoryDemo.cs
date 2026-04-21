namespace DesignPatterns.Patterns.AbstractFactory;

internal class AbstractFactoryDemo : IPatternDemo
{
    public string Name => "Abstract Factory";

    public void Run()
    {
        Console.WriteLine("Light theme:");
        new LoginScreen(new LightThemeFactory()).Render();
        Console.WriteLine();

        Console.WriteLine("Dark theme:");
        new LoginScreen(new DarkThemeFactory()).Render();
        Console.WriteLine();

        Console.WriteLine("Same LoginScreen class, different factory -> different family.");
    }
}
