using System.Text.RegularExpressions;

namespace Templater.Patterns;

public class RegexPattern : Pattern
{
    public RegexPattern(Regex regexp)
    {
        Execute = (text, position) =>
        {
            var match = regexp.Match(text.Substring(position));
            return match.Success
                ? new PatternResult(match.Value, position + match.Value.Length)
                : null;
        };
    }
}