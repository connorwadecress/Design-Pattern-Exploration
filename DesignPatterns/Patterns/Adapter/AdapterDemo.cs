namespace DesignPatterns.Patterns.Adapter;

internal class AdapterDemo : IPatternDemo
{
    public string Name => "Adapter";

    public void Run()
    {
        // Caller code only knows about IPaymentProcessor.
        // It doesn't care which implementation it gets.
        IPaymentProcessor modern = new ModernPaymentProcessor();
        IPaymentProcessor legacy = new LegacyStripeAdapter(new LegacyStripeClient());

        Console.WriteLine("Charging $49.99 via the native impl:");
        modern.Charge("alice@example.com", 49.99m);

        Console.WriteLine();
        Console.WriteLine("Charging $49.99 via the legacy library through the adapter:");
        legacy.Charge("bob@example.com", 49.99m);

        Console.WriteLine();
        Console.WriteLine("Same caller code - the adapter hid the API mismatch.");
    }
}
