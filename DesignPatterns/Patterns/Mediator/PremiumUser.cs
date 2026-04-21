namespace DesignPatterns.Patterns.Mediator;

/// <summary>
/// A different flavour of concrete colleague. Receives the same messages
/// as a StandardUser but renders them differently (fancier prefix).
///
/// Interview talking point:
///   Colleagues don't have to be the same type. The mediator broadcasts
///   once; each concrete colleague decides what to do with the message.
///   Adding this class did not require changing any other class —
///   not the mediator, not the base User, not StandardUser. That's
///   Open/Closed in action.
/// </summary>
internal class PremiumUser : User
{
    public PremiumUser(IChatRoomMediator mediator, string name)
        : base(mediator, name) { }

    public override void Receive(string message, User from)
    {
        Console.WriteLine($"    [{Name} ★]   message from {from.Name}: \"{message}\"  (premium inbox)");
    }
}
