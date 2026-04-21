namespace DesignPatterns.Patterns.Mediator;

/// <summary>
/// Demonstrates the Mediator pattern using a chat-room example.
///
/// Four scenarios:
///   1. Basic broadcast — three users exchange messages; nobody references
///      anyone else directly.
///   2. Add a new user mid-session — existing users are untouched; the new
///      one simply registers and joins the conversation.
///   3. Mixed user types — a StandardUser and a PremiumUser both receive
///      the same broadcast but render it differently. Shows colleagues
///      don't have to be the same type.
///   4. Interaction rule on the mediator — a "spam" filter lives on the
///      ChatRoom, not on any user. Shows that interaction logic belongs
///      on the mediator.
///
/// Interview talking points:
///   • Mediator replaces a mesh of peer-to-peer references with a
///     hub-and-spoke model. Users reference only the mediator.
///   • Loose coupling is the headline benefit. Adding/removing/renaming
///     a colleague type doesn't cascade across other colleagues.
///   • Mediator vs Facade: Facade is one-way (client -> facade -> subsystem,
///     subsystem unaware). Mediator is many-way (peers <-> mediator,
///     peers aware of the mediator).
///   • Mediator vs Observer: Observer is one-to-many broadcast from a
///     single subject. Mediator is many-to-many coordination through a hub.
///   • Real-world: MediatR in .NET applies this pattern to in-process
///     messaging — controllers dispatch commands/queries without
///     referencing the handlers directly.
/// </summary>
internal class MediatorDemo : IPatternDemo
{
    public string Name => "Mediator (chat room)";

    public void Run()
    {
        RunBasicBroadcast();
        Separator();
        RunAddUserMidSession();
        Separator();
        RunMixedUserTypes();
        Separator();
        RunMediatorRule();
        Separator();
        PrintSummary();
    }

    private static void RunBasicBroadcast()
    {
        Console.WriteLine("=== Scenario 1: Basic broadcast ===");
        Console.WriteLine("Three users; nobody holds a reference to anybody else.");
        Console.WriteLine();

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

    private static void RunAddUserMidSession()
    {
        Console.WriteLine("=== Scenario 2: New user joins mid-session ===");
        Console.WriteLine("No existing user class was changed. The new user just registers.");
        Console.WriteLine();

        var room = new ChatRoom();
        var alice = new StandardUser(room, "Alice");
        var bob = new StandardUser(room, "Bob");
        room.Register(alice);
        room.Register(bob);

        alice.Send("Anyone else up?");
        Console.WriteLine();

        // Add a new user — no change to Alice, Bob, or ChatRoom required.
        var dave = new StandardUser(room, "Dave");
        room.Register(dave);

        Console.WriteLine();
        dave.Send("Just me, late as usual.");
    }

    private static void RunMixedUserTypes()
    {
        Console.WriteLine("=== Scenario 3: Mixed colleague types ===");
        Console.WriteLine("PremiumUser receives the same broadcast as StandardUser but renders it differently.");
        Console.WriteLine("The mediator doesn't care which concrete type a colleague is.");
        Console.WriteLine();

        var room = new ChatRoom();
        var alice = new StandardUser(room, "Alice");
        var vip = new PremiumUser(room, "Eve");
        room.Register(alice);
        room.Register(vip);

        Console.WriteLine();
        alice.Send("Hello VIP!");
        Console.WriteLine();
        vip.Send("Greetings, plebs.");
    }

    private static void RunMediatorRule()
    {
        Console.WriteLine("=== Scenario 4: Interaction rule lives on the mediator ===");
        Console.WriteLine("ChatRoom filters messages containing \"spam\". Users aren't involved in that rule.");
        Console.WriteLine();

        var room = new ChatRoom();
        var alice = new StandardUser(room, "Alice");
        var bob = new StandardUser(room, "Bob");
        room.Register(alice);
        room.Register(bob);

        Console.WriteLine();
        alice.Send("Buy cheap spam now!");  // filtered by the mediator
        Console.WriteLine();
        alice.Send("Weather's nice today."); // delivered
    }

    private static void PrintSummary()
    {
        Console.WriteLine("=== Summary ===");
        Console.WriteLine("  Mediator replaces a mesh of peer-to-peer references with a hub.");
        Console.WriteLine("  Every user knows ONE thing about the outside world: the ChatRoom.");
        Console.WriteLine("  They don't know each other exists. Add one, remove one, rename one —");
        Console.WriteLine("  no other user is affected.");
        Console.WriteLine();
        Console.WriteLine("  Interaction logic (broadcast routing, spam filtering) lives on the");
        Console.WriteLine("  mediator, where it belongs — it's a property of the RELATIONSHIP,");
        Console.WriteLine("  not of any one colleague.");
        Console.WriteLine();
        Console.WriteLine("  Mediator vs Facade:");
        Console.WriteLine("    Facade     -> one-way, subsystem doesn't know about it.");
        Console.WriteLine("    Mediator   -> many-way, all peers know about it.");
        Console.WriteLine();
        Console.WriteLine("  Mediator vs Observer:");
        Console.WriteLine("    Observer   -> one-to-many broadcast from a single subject.");
        Console.WriteLine("    Mediator   -> many-to-many coordination through a hub.");
        Console.WriteLine();
        Console.WriteLine("  Real-world: MediatR (NuGet package) applies this pattern to in-process");
        Console.WriteLine("  messaging. Controllers dispatch requests to IMediator; handlers are");
        Console.WriteLine("  resolved automatically. It's the backbone of most CQRS .NET codebases.");
    }

    private static void Separator()
    {
        Console.WriteLine();
        Console.WriteLine(new string('-', 72));
        Console.WriteLine();
    }
}
