namespace DesignPatterns.Patterns.AbstractFactory;

/// <summary>
/// Concrete factory — produces the DARK family of widgets.
/// </summary>
internal class DarkThemeFactory : IUiFactory
{
    public IButton CreateButton() => new DarkButton();
    public ITextBox CreateTextBox() => new DarkTextBox();
}
