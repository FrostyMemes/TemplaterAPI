using System.Text;
using System.Text.RegularExpressions;

namespace Templater.Builder;

public class TemplateParser : TemplatePatterns
{
    private readonly TemplateBuilder builder;

    public TemplateParser()
    {
        builder = new TemplateBuilder();
    }

    public string Parse(string markdown)
    {
        StringBuilder content = new();
        List<string> keys = new();
        string tag, type, title, text;
        string literalKey, litrealBody;
        string[] literalParts = null;
        string[] options = null;
        var id = 0;
        

        builder.Clear();
        builder.AddTag("form");

        try
        {
            var literals = markdown
                .Split(';')
                .Where(literal => !string.IsNullOrWhiteSpace(literal))
                .Select(literal => literal.Trim())
                .ToArray();

            foreach (var literal in literals)
            {
                content.Clear();

                literalParts = literal.Split(':');
                literalKey = literalParts[0].Trim();
                litrealBody = literalParts[1].Trim();

                builder.AddTag("div");
                
                var classDiv = ptrRoundBraceContent.Execute(literalKey, 0);
                if (!IsNull(classDiv.Result))
                {
                    builder.AddAttribute("class", classDiv.Result);
                    literalKey = Regex.Replace(literalKey, @"\((.*)\)", "").Trim();
                }

                if (keys.Contains(literalKey))
                    throw new KeyExistingException($"The key ${literalKey} already exist");

                keys.Append(literalKey);
                title = literalKey;

                if (!IsNull(ptrMarksArea.Execute(litrealBody, 0).Result))
                {
                    var markGroup = ptrMarkGroupWords.Matches(litrealBody);
                    tag = markGroup.Count > 1 ? "textarea" : "input";

                    foreach (Match match in markGroup) 
                    { 
                        text = ptrMarksContent.Execute(match.Value, 0).Result;
                        content.Append(string.IsNullOrEmpty(text) ? "\n" : $"{text}\n");
                    }

                    builder
                        .AddTag(tag)
                        .AddAttribute("name", literalKey)
                        .AddAttribute("id", literalKey)
                        .AddAttribute("placeholder", title)
                        .AddText(tag == "input"
                            ? $" value={content}>"
                            : $"{content} </{tag}>")
                        .AddTag("label")
                        .AddAttribute("for", literalKey)
                        .AddText(title)
                        .AddTag("/label");
                }
                else
                {
                    options = litrealBody
                        .Split(',')
                        .Where(option => !string.IsNullOrWhiteSpace(option))
                        .Select(option => option.Trim())
                        .ToArray();

                    if (!IsNull(ptrVerticalBraceArea.Execute(options[0], 0).Result))
                    {
                        builder
                            .AddTag("label")
                            .AddAttribute("for", literalKey)
                            .AddText(title)
                            .AddTag("/label")
                            .AddTag("select")
                            .AddAttribute("name", literalKey)
                            .AddAttribute("id", literalKey)
                            .AddAttribute("aria-label", literalKey);

                        foreach (var option in options) 
                        {
                            if (!IsNull(ptrVerticalBraceArea.Execute(option, 0).Result))
                            {
                                var optionTemplate = ptrVerticalBraceContent.Execute(option, 0);
                                builder
                                    .AddTag("option")
                                    .AddAttribute("value", optionTemplate.Result)
                                    .AddText(optionTemplate.Result)
                                    .AddTag("/option");
                            }
                            builder.AddTag("/select");
                        }
                    }
                    else
                    {
                        id = 1;
                        type = IsNull(ptrRoundBraceArea.Execute(options[0], 0).Result)
                            ? "checkbox"
                            : "radio";

                        foreach (var option in options)
                        {
                            if (!IsNull(ptrEnumTags[type][0].Execute(option, 0).Result))
                            {
                                var temp = ptrEnumTags[type][1].Execute(option, 0);

                                var check = string.IsNullOrEmpty(temp.Result)
                                    ? string.Empty
                                    : "checked";

                                var optionLabel = option
                                    .Substring(temp.EndPosition + 2)
                                    .Trim();

                                builder
                                    .AddTag("input")
                                    .AddAttribute("type", type)
                                    .AddAttribute("id", $"{id}")
                                    .AddAttribute("name", literalKey)
                                    .AddAttribute(check)
                                    .AddTag("/input")
                                    .AddTag("label")
                                    .AddAttribute("for", $"{id}")
                                    .AddText(optionLabel)
                                    .AddTag("/label");
                                id++;
                            }
                        }
                    }
                }
                builder.AddTag("/div");
            }

            builder.AddTag("/form");
            return builder.Build();
        }
        catch (Exception e)
        {
            builder.Clear();
            builder
                .AddTag("div")
                .AddAttribute("class", "alert alert-danger")
                .AddAttribute("role", "alert")
                .AddText(e is KeyExistingException
                    ? e.Message
                    : "Error: check syntax")
                .AddTag("/div");

            return builder.Build();
        }
    }

    private bool IsNull(object? value)
    {
        return value == null;
    }
}