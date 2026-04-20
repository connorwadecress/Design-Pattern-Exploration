namespace DesignPatterns;

/// <summary>
/// Contract for every design-pattern demo in this project.
/// Each pattern folder will contain one class that implements this interface,
/// exposing a display name and a Run() method the menu can invoke.
/// </summary>
internal interface IPatternDemo
{
    /// <summary>
    /// Human-readable name shown in the console menu (e.g. "Singleton").
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Executes the pattern's demonstration.
    /// </summary>
    void Run();
}
