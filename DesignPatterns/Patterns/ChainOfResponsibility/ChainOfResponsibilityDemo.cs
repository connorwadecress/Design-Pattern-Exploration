namespace DesignPatterns.Patterns.ChainOfResponsibility;

internal class ChainOfResponsibilityDemo : IPatternDemo
{
    public string Name => "Chain of Responsibility";

    public void Run()
    {
        // Build the chain: Manager -> Director -> CFO.
        // SetNext returns the new tail so we can keep wiring fluently.
        var manager = new ManagerApprover();
        var director = new DirectorApprover();
        var cfo = new CfoApprover();
        manager.SetNext(director).SetNext(cfo);

        // Caller only ever talks to the head of the chain.
        var requests = new[]
        {
            new ExpenseRequest("Alice",     250m,  "team lunch"),
            new ExpenseRequest("Bob",     5_000m,  "monitor refresh"),
            new ExpenseRequest("Carol",  75_000m,  "conference sponsorship"),
            new ExpenseRequest("Dave", 250_000m,   "office relocation"),
        };

        foreach (var r in requests)
        {
            Console.WriteLine($"Request: {r.Submitter} wants ${r.Amount} for {r.Reason}");
            manager.Handle(r);
            Console.WriteLine();
        }

        Console.WriteLine("Each handler either approves or forwards - no handler knows the others' rules.");
    }
}
