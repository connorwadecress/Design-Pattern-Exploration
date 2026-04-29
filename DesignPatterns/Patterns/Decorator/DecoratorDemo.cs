namespace DesignPatterns.Patterns.Decorator;

internal class DecoratorDemo : IPatternDemo
{
    public string Name => "Decorator";

    public void Run()
    {
        // 1. Plain - no decorators.
        INotifier plain = new EmailNotifier();
        Console.WriteLine("Plain notifier:");
        plain.Send("hello");

        Console.WriteLine();

        // 2. One decorator wrapping the email notifier.
        INotifier withTime = new TimestampDecorator(new EmailNotifier());
        Console.WriteLine("With timestamp decorator:");
        withTime.Send("hello");

        Console.WriteLine();

        // 3. Three stacked decorators. Outer-to-inner order matters:
        // Logging runs first (outermost), then Retry runs the inner Send twice,
        // and inside each retry the Timestamp prefixes the message before Email prints.
        INotifier stacked = new LoggingDecorator(
                                new RetryDecorator(
                                    new TimestampDecorator(
                                        new EmailNotifier()),
                                    attempts: 2));
        Console.WriteLine("Stacked: Logging(Retry(Timestamp(Email))):");
        stacked.Send("hello");

        Console.WriteLine();
        Console.WriteLine("Same INotifier interface every layer - that's why they stack.");
    }
}
