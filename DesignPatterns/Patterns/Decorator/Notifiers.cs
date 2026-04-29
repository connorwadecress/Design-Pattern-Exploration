namespace DesignPatterns.Patterns.Decorator;

// The component interface. Every notifier - real or decorator - looks like this.
internal interface INotifier
{
    void Send(string message);
}

// The "real" component. Decorators wrap one of these.
internal class EmailNotifier : INotifier
{
    public void Send(string message)
        => Console.WriteLine($"  [Email] {message}");
}

// Abstract base for decorators. Holds the inner notifier and forwards Send.
// Concrete decorators override Send to add behaviour around base.Send.
internal abstract class NotifierDecorator : INotifier
{
    protected readonly INotifier Inner;
    protected NotifierDecorator(INotifier inner) => Inner = inner;

    public virtual void Send(string message) => Inner.Send(message);
}

// Adds [time] in front of the message before delegating.
internal class TimestampDecorator : NotifierDecorator
{
    public TimestampDecorator(INotifier inner) : base(inner) { }

    public override void Send(string message)
    {
        var stamped = $"[{DateTime.Now:HH:mm:ss}] {message}";
        Inner.Send(stamped);
    }
}

// Logs the call before AND after - both sides of the inner call.
internal class LoggingDecorator : NotifierDecorator
{
    public LoggingDecorator(INotifier inner) : base(inner) { }

    public override void Send(string message)
    {
        Console.WriteLine($"  [Log]   sending: \"{message}\"");
        Inner.Send(message);
        Console.WriteLine($"  [Log]   sent");
    }
}

// Sends the message twice - the simplest possible "augment" behaviour
// to show that decorators can change behaviour, not just observe it.
internal class RetryDecorator : NotifierDecorator
{
    private readonly int _attempts;
    public RetryDecorator(INotifier inner, int attempts) : base(inner) => _attempts = attempts;

    public override void Send(string message)
    {
        for (int i = 1; i <= _attempts; i++)
        {
            Console.WriteLine($"  [Retry] attempt {i}/{_attempts}");
            Inner.Send(message);
        }
    }
}
