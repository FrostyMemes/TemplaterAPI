namespace Templater.Services.MarkdownTemplateService.Builder.Patterns.Combinators;

public class AlternativePattern : Pattern
{
    public AlternativePattern(Pattern pattern, PatternResult? alternative)
    {
        Execute = (text, positiоn) => { return pattern.Execute(text, positiоn) ?? alternative; };
    }
}