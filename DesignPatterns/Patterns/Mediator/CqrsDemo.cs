namespace DesignPatterns.Patterns.Mediator;

internal class CqrsDemo : IPatternDemo
{
    public string Name => "Mediator (CQRS)";

    public void Run()
    {
        // 1. Build the shared store and the mediator.
        var store = new UserStore();
        var mediator = new CqrsMediator();

        // 2. Register one handler per message type.
        mediator.RegisterCommand(new CreateUserHandler(store));
        mediator.RegisterQuery<GetUserByIdQuery, UserDto?>(new GetUserByIdHandler(store));
        mediator.RegisterQuery<CountUsersQuery, int>(new CountUsersHandler(store));

        Console.WriteLine("Two commands and two queries through the mediator:");
        Console.WriteLine();

        // 3. Commands - writes that return nothing.
        mediator.Send(new CreateUserCommand("Alice", "alice@example.com"));
        mediator.Send(new CreateUserCommand("Bob", "bob@example.com"));

        Console.WriteLine();

        // 4. Queries - reads that return data.
        var found = mediator.Send<GetUserByIdQuery, UserDto?>(new GetUserByIdQuery(1));
        var missing = mediator.Send<GetUserByIdQuery, UserDto?>(new GetUserByIdQuery(99));
        var total = mediator.Send<CountUsersQuery, int>(new CountUsersQuery());

        Console.WriteLine();
        Console.WriteLine($"  caller saw: found={found?.Name ?? "null"}, missing={missing?.Name ?? "null"}, total={total}");
        Console.WriteLine();
        Console.WriteLine("Caller depends on ICqrsMediator only - no direct handler references.");
        Console.WriteLine("Same flavour as the chat-room mediator, but messages are typed and have ONE handler each.");
    }
}
