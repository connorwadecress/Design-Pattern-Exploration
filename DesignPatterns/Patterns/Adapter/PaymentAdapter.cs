namespace DesignPatterns.Patterns.Adapter;

// The interface our app already speaks. Every caller in the codebase uses this.
internal interface IPaymentProcessor
{
    bool Charge(string customerEmail, decimal amount);
}

// A "modern" processor we already had - implements the interface natively.
// Included so the demo can show how an adapter sits next to a native impl.
internal class ModernPaymentProcessor : IPaymentProcessor
{
    public bool Charge(string customerEmail, decimal amount)
    {
        Console.WriteLine($"  [Modern]  charged {customerEmail} ${amount}");
        return true;
    }
}

// The third-party library we can't change. Different method name,
// different argument shape (cents instead of dollars), different return type.
// example would be in a nuget package
internal class LegacyStripeClient
{
    public string ExecutePayment(int amountInCents, string currencyCode, string reference)
    {
        Console.WriteLine($"  [Legacy]  executed {amountInCents}c {currencyCode} ref={reference}");
        // Real library would return a status object - we mimic that with a string.
        return "OK";
    }
}

// THE ADAPTER. Implements the interface our code expects and holds the legacy
// client. Its only job is translation - dollars to cents, our method to theirs,
// their string status back to our bool.
internal class LegacyStripeAdapter : IPaymentProcessor
{
    private readonly LegacyStripeClient _legacy;

    public LegacyStripeAdapter(LegacyStripeClient legacy) => _legacy = legacy;

    public bool Charge(string customerEmail, decimal amount)
    {
        var cents = (int)(amount * 100);
        var reference = $"ref-{customerEmail}";
        var status = _legacy.ExecutePayment(cents, "USD", reference);
        return status == "OK";
    }
}
