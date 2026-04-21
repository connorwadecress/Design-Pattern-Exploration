namespace DesignPatterns.Patterns.Mediator;

internal class MediatorDemo : IPatternDemo
{
    public string Name => "Mediator";

    public void Run()
    {
        var room = new ChatRoom();
        var alice = new StandardUser(room, "Alice");
        var bob = new StandardUser(room, "Bob");
        var carol = new StandardUser(room, "Carol");

        room.Register(alice);
        room.Register(bob);
        room.Register(carol);
        Console.WriteLine();

        alice.Send("Good morning everyone.");
        Console.WriteLine();
        bob.Send("Morning Alice!");
    }
}
