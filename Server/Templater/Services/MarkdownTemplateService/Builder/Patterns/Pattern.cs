namespace Templater.Patterns;

public abstract class Pattern
{
    public delegate PatternResult? ExecutePattern(string text, int positiоn);
    public ExecutePattern Execute { get; init; }
}