namespace DesignPatterns.Patterns.ChainOfResponsibility;

internal record ExpenseRequest(string Submitter, decimal Amount, string Reason);

// Abstract handler. Holds a reference to the NEXT handler in the chain.
// Concrete handlers either approve, or call base.Handle to forward.
internal abstract class Approver
{
    private Approver? _next;

    // Returns the same approver so SetNext calls can be chained on construction.
    public Approver SetNext(Approver next)
    {
        _next = next;
        return next;
    }

    public virtual void Handle(ExpenseRequest request)
    {
        if (_next is not null)
        {
            _next.Handle(request);
        }
        else
        {
            // End of chain - nobody could approve.
            Console.WriteLine($"  [REJECTED] no one could approve ${request.Amount} ({request.Reason})");
        }
    }
}

internal class ManagerApprover : Approver
{
    private const decimal Limit = 1_000m;

    public override void Handle(ExpenseRequest request)
    {
        if (request.Amount <= Limit)
        {
            Console.WriteLine($"  [Manager]  approved ${request.Amount} for {request.Submitter} ({request.Reason})");
            return;
        }
        Console.WriteLine($"  [Manager]  ${request.Amount} > ${Limit} - escalating");
        base.Handle(request);
    }
}

internal class DirectorApprover : Approver
{
    private const decimal Limit = 10_000m;

    public override void Handle(ExpenseRequest request)
    {
        if (request.Amount <= Limit)
        {
            Console.WriteLine($"  [Director] approved ${request.Amount} for {request.Submitter} ({request.Reason})");
            return;
        }
        Console.WriteLine($"  [Director] ${request.Amount} > ${Limit} - escalating");
        base.Handle(request);
    }
}

internal class CfoApprover : Approver
{
    private const decimal Limit = 100_000m;

    public override void Handle(ExpenseRequest request)
    {
        if (request.Amount <= Limit)
        {
            Console.WriteLine($"  [CFO]      approved ${request.Amount} for {request.Submitter} ({request.Reason})");
            return;
        }
        Console.WriteLine($"  [CFO]      ${request.Amount} > ${Limit} - escalating");
        base.Handle(request);
    }
}
