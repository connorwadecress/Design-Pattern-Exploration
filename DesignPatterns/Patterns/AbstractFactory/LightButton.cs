namespace DesignPatterns.Patterns.AbstractFactory;

/// <summary>
/// Concrete product — part of the LIGHT theme family.
/// </summary>
internal class LightButton : IButton
{
    public void Render(string label)
    {
        // Light theme: minimal, airy, lowercase, thin borders.
        Console.WriteLine($"    [ {label.ToLowerInvariant(),-10} ]     (light button)");
    }
}
