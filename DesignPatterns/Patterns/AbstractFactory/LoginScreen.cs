namespace DesignPatterns.Patterns.AbstractFactory;

/// <summary>
/// CLIENT of the Abstract Factory.
///
/// This class is the whole reason the pattern earns its keep:
///   • It depends on the IUiFactory ABSTRACTION (constructor-injected) —
///     Dependency Inversion Principle at work.
///   • It knows nothing about light vs dark themes.
///   • Its rendering code is EXACTLY THE SAME regardless of theme —
///     swap the factory, everything else is identical.
///
/// In the interview, point at this class and say:
///   "This is what the pattern buys you — client code unchanged across families."
/// </summary>
internal class LoginScreen
{
    private readonly IUiFactory _factory;

    public LoginScreen(IUiFactory factory)
    {
        _factory = factory;
    }

    public void Render()
    {
        // Ask the factory for widgets. We don't know or care which theme
        // this factory represents — but every widget we get back is
        // guaranteed to belong to the same family.
        var usernameBox = _factory.CreateTextBox();
        var passwordBox = _factory.CreateTextBox();
        var submitButton = _factory.CreateButton();

        Console.WriteLine("    +--- Login ---+");
        usernameBox.Render("Username");
        passwordBox.Render("Password");
        submitButton.Render("Submit");
        Console.WriteLine("    +-------------+");
    }
}
