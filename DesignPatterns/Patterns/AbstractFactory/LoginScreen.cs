namespace DesignPatterns.Patterns.AbstractFactory;

// Client. Depends on IUiFactory - knows nothing about which theme it's rendering.
// The same Render() method produces a light OR dark screen depending on the injected factory.
internal class LoginScreen
{
    private readonly IUiFactory _factory;

    public LoginScreen(IUiFactory factory) => _factory = factory;

    public void Render()
    {
        var username = _factory.CreateTextBox();
        var password = _factory.CreateTextBox();
        var submit = _factory.CreateButton();

        Console.WriteLine("    +--- Login ---+");
        username.Render("Username");
        password.Render("Password");
        submit.Render("Submit");
        Console.WriteLine("    +-------------+");
    }
}
