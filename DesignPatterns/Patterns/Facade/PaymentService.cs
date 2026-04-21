namespace DesignPatterns.Patterns.Facade;

/// <summary>
/// Subsystem service — responsible for charging customers.
/// In a real app this would talk to Stripe, a bank gateway, etc.
/// </summary>
internal interface IPaymentService
{
    PaymentResult Charge(Customer customer, decimal amount);
}

internal class PaymentService : IPaymentService
{
    private readonly bool _simulateFailure;

    /// <summary>
    /// Construct the service.
    /// </summary>
    /// <param name="simulateFailure">
    /// When true, all charges will fail — used by the demo to show the
    /// facade's rollback behaviour. In a real app this would not exist.
    /// </param>
    public PaymentService(bool simulateFailure = false)
    {
        _simulateFailure = simulateFailure;
    }

    public PaymentResult Charge(Customer customer, decimal amount)
    {
        Console.WriteLine($"    [Payment]       Charging {customer.Name} ${amount}...");

        if (_simulateFailure)
        {
            Console.WriteLine("    [Payment]       DECLINED.");
            return new PaymentResult(false, Reference: string.Empty, FailureReason: "Card declined");
        }

        var reference = $"PAY-{Guid.NewGuid().ToString()[..8]}";
        Console.WriteLine($"    [Payment]       Charged successfully. Reference {reference}.");
        return new PaymentResult(true, reference, FailureReason: null);
    }
}
