namespace DesignPatterns.Patterns.Mediator;

/// <summary>
/// Abstract COLLEAGUE base class.
///
/// Every concrete user holds a reference to the mediator (and ONLY the
/// mediator) and has a name. They can:
///   • Send a message (by asking the mediator to deliver it).
///   • Receive a message (implementation varies per concrete user).
///
/// Notice what this class does NOT have:
///   • No reference to any other User.
///   • No list of peers.
///   • No knowledge of how many users exist or what they're doing.
///
/// Everything outward-facing goes through the mediator. That's the
/// loose-coupling benefit the pattern exists to provide.
/// </summary>
internal abstract class User
{
    protected readonly IChatRoomMediator Mediator;

    public string Name { get; }

    protected User(IChatRoomMediator mediator, string name)
    {
        Mediator = mediator;
        Name = name;
    }

    /// <summary>
    /// Send a message. The user doesn't know who will receive it — that's
    /// the mediator's job.
    /// </summary>
    public void Send(string message)
    {
        Console.WriteLine($"    [{Name}]  >> sends: \"{message}\"");
        Mediator.SendMessage(message, from: this);
    }

    /// <summary>
    /// Receive a message delivered by the mediator.
    /// Abstract — different concrete users render incoming messages differently.
    /// </summary>
    public abstract void Receive(string message, User from);
}
