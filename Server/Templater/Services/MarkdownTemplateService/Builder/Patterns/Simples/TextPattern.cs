namespace Templater.Services.MarkdownTemplateService.Builder.Patterns.Simples;

public class TextPattern : Pattern
{
    public TextPattern(string pattern)
    {
        Execute = (text, position) =>
        {
            return text.Substring(position, pattern.Length) == pattern
                ? new PatternResult(pattern, position + pattern.Length)
                : null;
        };
    }
}