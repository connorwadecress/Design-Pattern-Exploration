namespace DesignPatterns.Patterns.Mediator;

// Abstract colleague. Every user holds ONLY a reference to the mediator,
// not to any other user. That's the whole point of the pattern.
internal abstract class User
{
    protected readonly IChatRoomMediator Mediator;
    public string Name { get; }

    protected User(IChatRoomMediator mediator, string name)
    {
        Mediator = mediator;
        Name = name;
    }

    public void Send(string message) => Mediator.SendMessage(message, from: this);

    public abstract void Receive(string message, User from);
}

internal class StandardUser : User
{
    public StandardUser(IChatRoomMediator mediator, string name) : base(mediator, name) { }

    public override void Receive(string message, User from)
        => Console.WriteLine($"    [{Name}] got \"{message}\" from {from.Name}");
}
