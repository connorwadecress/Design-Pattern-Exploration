namespace DesignPatterns.Patterns.Mediator;

/// <summary>
/// The CONCRETE MEDIATOR.
///
/// Owns the list of registered users and all the interaction logic:
///   • how messages are routed (broadcast to everyone except the sender);
///   • any rules applied to messages (here: a crude "spam" filter to show
///     that interaction rules live in the mediator, not in the users).
///
/// The interesting thing is what this class ENABLES:
///   • Users don't know about each other.
///   • Adding a new user type (see PremiumUser) didn't require changing
///     this class.
///   • Changing how messages are routed (broadcast vs direct vs filtered)
///     only changes this class.
/// </summary>
internal class ChatRoom : IChatRoomMediator
{
    private readonly List<User> _users = new();

    public void Register(User user)
    {
        _users.Add(user);
        Console.WriteLine($"    [ChatRoom]   {user.Name} joined.");
    }

    public void SendMessage(string message, User from)
    {
        // Interaction rule #1 — sender does not receive their own message.
        // Interaction rule #2 — crude spam filter. The point is that a rule
        // like this lives HERE, on the mediator, not on every individual user.
        if (message.Contains("spam", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"    [ChatRoom]   Blocked a message from {from.Name} (contained \"spam\").");
            return;
        }

        Console.WriteLine($"    [ChatRoom]   Routing message from {from.Name}: \"{message}\"");

        foreach (var user in _users)
        {
            if (user == from) continue;
            user.Receive(message, from);
        }
    }
}
