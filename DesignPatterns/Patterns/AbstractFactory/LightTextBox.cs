namespace DesignPatterns.Patterns.AbstractFactory;

/// <summary>
/// Concrete product — part of the LIGHT theme family.
/// </summary>
internal class LightTextBox : ITextBox
{
    public void Render(string label)
    {
        // Light theme: underline-style input field.
        Console.WriteLine($"    {label,-10} : ________________     (light textbox)");
    }
}
