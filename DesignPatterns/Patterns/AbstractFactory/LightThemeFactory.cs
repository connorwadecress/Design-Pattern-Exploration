namespace DesignPatterns.Patterns.AbstractFactory;

/// <summary>
/// Concrete factory — produces the LIGHT family of widgets.
///
/// Note the guarantee this class provides: every method returns a widget
/// that belongs to the light theme. There is no way to obtain a dark widget
/// from this factory.
/// </summary>
internal class LightThemeFactory : IUiFactory
{
    public IButton CreateButton() => new LightButton();
    public ITextBox CreateTextBox() => new LightTextBox();
}
