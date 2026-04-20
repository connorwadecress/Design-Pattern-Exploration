namespace DesignPatterns.Patterns.AbstractFactory;

/// <summary>
/// Concrete product — part of the DARK theme family.
/// </summary>
internal class DarkTextBox : ITextBox
{
    public void Render(string label)
    {
        // Dark theme: solid-fill input field.
        Console.WriteLine($"    {label.ToUpperInvariant(),-10} : ################     (dark textbox)");
    }
}
