namespace Templater.Services.MarkdownTemplateService.Builder.Patterns.Combinators;

public class AnyPattern : Pattern
{
    public AnyPattern(params Pattern[] patterns)
    {
        Execute = (text, positiоn) =>
        {
            PatternResult? result = null;

            foreach (var pattern in patterns)
                if ((result = pattern.Execute(text, positiоn)) != null)
                    break;

            return result;
        };
    }
}