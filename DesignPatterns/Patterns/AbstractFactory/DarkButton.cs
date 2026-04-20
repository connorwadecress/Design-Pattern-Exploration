namespace DesignPatterns.Patterns.AbstractFactory;

/// <summary>
/// Concrete product — part of the DARK theme family.
/// </summary>
internal class DarkButton : IButton
{
    public void Render(string label)
    {
        // Dark theme: bold, blocky, uppercase.
        Console.WriteLine($"    ### {label.ToUpperInvariant(),-10} ###   (dark button)");
    }
}
