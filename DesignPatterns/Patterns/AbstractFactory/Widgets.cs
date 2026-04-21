namespace DesignPatterns.Patterns.AbstractFactory;

// Abstract products - contracts the client codes against.
internal interface IButton { void Render(string label); }
internal interface ITextBox { void Render(string label); }

// Light family.
internal class LightButton : IButton
{
    public void Render(string label)
        => Console.WriteLine($"    [ {label.ToLower(),-10} ]   (light button)");
}

internal class LightTextBox : ITextBox
{
    public void Render(string label)
        => Console.WriteLine($"    {label,-10} : ________   (light textbox)");
}

// Dark family.
internal class DarkButton : IButton
{
    public void Render(string label)
        => Console.WriteLine($"    ### {label.ToUpper(),-10} ###   (dark button)");
}

internal class DarkTextBox : ITextBox
{
    public void Render(string label)
        => Console.WriteLine($"    {label.ToUpper(),-10} : ########   (dark textbox)");
}

// Abstract factory - one creation method per product type.
// A concrete factory guarantees that every widget it returns is from the same family.
internal interface IUiFactory
{
    IButton CreateButton();
    ITextBox CreateTextBox();
}

internal class LightThemeFactory : IUiFactory
{
    public IButton CreateButton() => new LightButton();
    public ITextBox CreateTextBox() => new LightTextBox();
}

internal class DarkThemeFactory : IUiFactory
{
    public IButton CreateButton() => new DarkButton();
    public ITextBox CreateTextBox() => new DarkTextBox();
}
