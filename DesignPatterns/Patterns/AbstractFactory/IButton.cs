namespace DesignPatterns.Patterns.AbstractFactory;

/// <summary>
/// Abstract product. Every theme will provide a concrete button that can
/// render itself. Client code only ever talks to this interface — it never
/// knows or cares which concrete button it's rendering.
/// </summary>
internal interface IButton
{
    void Render(string label);
}
