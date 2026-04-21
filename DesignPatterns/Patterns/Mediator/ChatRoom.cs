namespace DesignPatterns.Patterns.Mediator;

// Mediator interface.
internal interface IChatRoomMediator
{
    void Register(User user);
    void SendMessage(string message, User from);
}

// Concrete mediator. Holds the list of users and owns the interaction logic.
// Users never reference each other - they all reference this.
internal class ChatRoom : IChatRoomMediator
{
    private readonly List<User> _users = new();

    public void Register(User user)
    {
        _users.Add(user);
        Console.WriteLine($"  {user.Name} joined");
    }

    public void SendMessage(string message, User from)
    {
        Console.WriteLine($"  {from.Name}: \"{message}\"");
        foreach (var user in _users)
        {
            if (user == from) continue;
            user.Receive(message, from);
        }
    }
}
