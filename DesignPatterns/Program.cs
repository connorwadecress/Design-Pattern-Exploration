using DesignPatterns;
using DesignPatterns.Patterns.Singleton;
using DesignPatterns.Patterns.Builder;
using DesignPatterns.Patterns.AbstractFactory;
using DesignPatterns.Patterns.Facade;
using DesignPatterns.Patterns.Strategy;
using DesignPatterns.Patterns.State;

// ---------------------------------------------------------------------------
// Registry of pattern demos.
// As each pattern is built, add a new entry to this list — nothing else in
// Program.cs needs to change, because the menu is driven entirely off the
// IPatternDemo contract (polymorphism / Liskov Substitution Principle).
// ---------------------------------------------------------------------------
var demos = new List<IPatternDemo>
{
    new SingletonDemo(),
    new BuilderDemo(),
    new AbstractFactoryDemo(),
    new FacadeDemo(),
    new StrategyDemo(),
    new StateDemo(),
    // new MediatorDemo(),
};

while (true)
{
    Console.Clear();
    Console.WriteLine("=== Design Patterns Demo ===");
    Console.WriteLine();

    if (demos.Count == 0)
    {
        Console.WriteLine("(No pattern demos registered yet — add one to the list in Program.cs.)");
    }
    else
    {
        for (int i = 0; i < demos.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {demos[i].Name}");
        }
    }

    Console.WriteLine("  0. Exit");
    Console.WriteLine();
    Console.Write("Choose a pattern: ");

    var input = Console.ReadLine();

    if (!int.TryParse(input, out var choice))
    {
        Console.WriteLine("Please enter a number.");
        Pause();
        continue;
    }

    if (choice == 0)
    {
        break;
    }

    if (choice < 1 || choice > demos.Count)
    {
        Console.WriteLine("Invalid choice.");
        Pause();
        continue;
    }

    var demo = demos[choice - 1];

    Console.Clear();
    Console.WriteLine($"--- Running: {demo.Name} ---");
    Console.WriteLine();

    demo.Run();

    Console.WriteLine();
    Console.WriteLine("--- Demo complete ---");
    Pause();
}

static void Pause()
{
    Console.WriteLine();
    Console.Write("Press any key to continue...");
    Console.ReadKey(intercept: true);
}
