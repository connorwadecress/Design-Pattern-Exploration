using DesignPatterns;
using DesignPatterns.Patterns.Singleton;
using DesignPatterns.Patterns.Builder;
using DesignPatterns.Patterns.AbstractFactory;
using DesignPatterns.Patterns.Facade;
using DesignPatterns.Patterns.Strategy;
using DesignPatterns.Patterns.State;
using DesignPatterns.Patterns.Mediator;
using DesignPatterns.Patterns.Adapter;
using DesignPatterns.Patterns.Decorator;
using DesignPatterns.Patterns.Proxy;
using DesignPatterns.Patterns.ChainOfResponsibility;
using DesignPatterns.Reflection;

var demos = new List<IPatternDemo>
{
    new SingletonDemo(),
    new BuilderDemo(),
    new AbstractFactoryDemo(),
    new FacadeDemo(),
    new StrategyDemo(),
    new StateDemo(),
    new MediatorDemo(),
    new CqrsDemo(),
    new AdapterDemo(),
    new DecoratorDemo(),
    new ProxyDemo(),
    new ChainOfResponsibilityDemo(),
    new ReflectionDemo(),
};

while (true)
{
    Console.Clear();
    Console.WriteLine("=== Design Patterns ===");
    Console.WriteLine();
    for (int i = 0; i < demos.Count; i++)
        Console.WriteLine($"  {i + 1}. {demos[i].Name}");
    Console.WriteLine("  0. Exit");
    Console.WriteLine();
    Console.Write("Choose: ");

    if (!int.TryParse(Console.ReadLine(), out var choice)) continue;
    if (choice == 0) break;
    if (choice < 1 || choice > demos.Count) continue;

    var demo = demos[choice - 1];

    Console.Clear();
    Console.WriteLine($"--- {demo.Name} ---");
    Console.WriteLine();
    demo.Run();
    Console.WriteLine();
    Console.Write("Press any key...");
    Console.ReadKey(true);
}
