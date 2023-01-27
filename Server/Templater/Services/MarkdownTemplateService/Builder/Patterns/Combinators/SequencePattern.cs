namespace Templater.Services.MarkdownTemplateService.Builder.Patterns.Combinators;

public class SequencePattern : Pattern
{
    public SequencePattern(params Pattern[] patterns)
    {
        Execute = (text, positiоn) =>
        {
            PatternResult? result = null;
            var currentPosition = positiоn;

            foreach (var pattern in patterns)
            {
                if ((result = pattern.Execute(text, currentPosition)) == null)
                    return null;

                currentPosition = positiоn;
            }

            return result;
        };
    }
}