namespace DesignPatterns.Patterns.AbstractFactory;

/// <summary>
/// Abstract product. Paired with <see cref="IButton"/> in the same "family" —
/// they must always come from the same theme, which is what the factory enforces.
/// </summary>
internal interface ITextBox
{
    void Render(string label);
}
