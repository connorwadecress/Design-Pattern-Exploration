namespace DesignPatterns;

// Every pattern demo implements this. Lets Program.cs hold a List<IPatternDemo>
// and build the menu without knowing about the concrete demo types.
internal interface IPatternDemo
{
    string Name { get; }
    void Run();
}
