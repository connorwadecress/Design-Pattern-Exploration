namespace DesignPatterns.Patterns.Mediator;

/// <summary>
/// A basic concrete colleague. Receives messages and logs them plainly.
/// </summary>
internal class StandardUser : User
{
    public StandardUser(IChatRoomMediator mediator, string name)
        : base(mediator, name) { }

    public override void Receive(string message, User from)
    {
        Console.WriteLine($"    [{Name}]     received from {from.Name}: \"{message}\"");
    }
}
