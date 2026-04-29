namespace DesignPatterns.Patterns.Mediator;

// CQRS message marker interfaces.
// IQuery<TResult> returns data; ICommand returns nothing (it's a state change).
internal interface IQuery<TResult> { }
internal interface ICommand { }

// Handler interfaces - one method, takes the message, returns the result (or void).
internal interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    TResult Handle(TQuery query);
}

internal interface ICommandHandler<TCommand> where TCommand : ICommand
{
    void Handle(TCommand command);
}

// The CQRS mediator. Same idea as ChatRoom (a hub) but typed:
// every message has ONE registered handler, picked by message type.
internal interface ICqrsMediator
{
    void RegisterCommand<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand;
    void RegisterQuery<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler) where TQuery : IQuery<TResult>;
    void Send<TCommand>(TCommand command) where TCommand : ICommand;
    TResult Send<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
}

internal class CqrsMediator : ICqrsMediator
{
    // One bag of handlers, keyed by message type.
    // The handler instance is stored as object because the generic type
    // varies per registration - we cast back at dispatch time.
    private readonly Dictionary<Type, object> _handlers = new();

    public void RegisterCommand<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand
        => _handlers[typeof(TCommand)] = handler;

    public void RegisterQuery<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler) where TQuery : IQuery<TResult>
        => _handlers[typeof(TQuery)] = handler;

    public void Send<TCommand>(TCommand command) where TCommand : ICommand
    {
        if (!_handlers.TryGetValue(typeof(TCommand), out var raw))
            throw new InvalidOperationException($"No handler for {typeof(TCommand).Name}");
        ((ICommandHandler<TCommand>)raw).Handle(command);
    }

    public TResult Send<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
    {
        if (!_handlers.TryGetValue(typeof(TQuery), out var raw))
            throw new InvalidOperationException($"No handler for {typeof(TQuery).Name}");
        return ((IQueryHandler<TQuery, TResult>)raw).Handle(query);
    }
}

// ----- Domain: a tiny in-memory user store -----

internal record UserDto(int Id, string Name, string Email);

// "Database" - a single dictionary the handlers share.
internal class UserStore
{
    private readonly Dictionary<int, UserDto> _users = new();
    private int _nextId = 1;

    public UserDto Add(string name, string email)
    {
        var user = new UserDto(_nextId++, name, email);
        _users[user.Id] = user;
        return user;
    }

    public UserDto? Find(int id) => _users.TryGetValue(id, out var u) ? u : null;
    public IReadOnlyCollection<UserDto> All() => _users.Values;
}

// ----- Commands (write side) -----

internal record CreateUserCommand(string Name, string Email) : ICommand;

internal class CreateUserHandler : ICommandHandler<CreateUserCommand>
{
    private readonly UserStore _store;
    public CreateUserHandler(UserStore store) => _store = store;

    public void Handle(CreateUserCommand command)
    {
        var created = _store.Add(command.Name, command.Email);
        Console.WriteLine($"  [CMD] CreateUser  -> id={created.Id} name={created.Name}");
    }
}

// ----- Queries (read side) -----

internal record GetUserByIdQuery(int Id) : IQuery<UserDto?>;

internal class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserDto?>
{
    private readonly UserStore _store;
    public GetUserByIdHandler(UserStore store) => _store = store;

    public UserDto? Handle(GetUserByIdQuery query)
    {
        var user = _store.Find(query.Id);
        Console.WriteLine($"  [QRY] GetUserById id={query.Id} -> {(user is null ? "not found" : user.Name)}");
        return user;
    }
}

internal record CountUsersQuery() : IQuery<int>;

internal class CountUsersHandler : IQueryHandler<CountUsersQuery, int>
{
    private readonly UserStore _store;
    public CountUsersHandler(UserStore store) => _store = store;

    public int Handle(CountUsersQuery query)
    {
        var count = _store.All().Count;
        Console.WriteLine($"  [QRY] CountUsers          -> {count}");
        return count;
    }
}
