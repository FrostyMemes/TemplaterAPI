namespace Templater.Services.MarkdownTemplateService.Builder.Patterns.Combinators;

public class ExceptionPattern : Pattern
{
    public ExceptionPattern(Pattern pattern, Pattern exception)
    {
        Execute = (text, positiоn) =>
        {
            PatternResult? result = null;
            
            if ((result = pattern.Execute(text, positiоn)) != null
                && exception.Execute(text, positiоn) == null)
                return result;
            
            return null;
        };
    }
}