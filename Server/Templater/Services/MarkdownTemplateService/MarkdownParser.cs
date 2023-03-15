using System.Text;
using System.Text.RegularExpressions;
using StackExchange.Redis;
using Templater.Services.MarkdownTemplateService.Builder;
using Templater.Services.MarkdownTemplateService.Builder.Patterns;
using Templater.Services.MarkdownTemplateService.Builder.Patterns.Simples;
using Templater.Services.MarkdownTemplateService.Builder.Patterns.Combinators;

namespace Templater.Services.MarkdownTemplateService;

public class MarkdownParser: IMarkdownParser
{
    static readonly PatternResult VoidResult = new (null, -1);
    
    public static readonly Regex ptrMarkGroupWords = new (
        @"""([^""\\]*(\\.[^""\\]*)*)""|\'([^\'\\]*(\\.[^\'\\]*)*)\'");
    
    public static readonly AlternativePattern ptrSquareBraceArea = new (
        new RegexPattern(
            new Regex(@"\[(.*)\]")), VoidResult);

    public static readonly AlternativePattern ptrRoundBraceArea = new (
        new RegexPattern(
            new Regex(@"\((.*)\)")), VoidResult);

    public static readonly AlternativePattern ptrVerticalBraceArea = new (
        new RegexPattern(
            new Regex(@"\|(.*)\|")), VoidResult);

    public static readonly AlternativePattern ptrFigureBraceArea = new (
        new RegexPattern(
            new Regex(@"\{(.*)\}")), VoidResult);
    
    public static readonly AlternativePattern ptrSingleMarkArea = new (
        new RegexPattern(
            new Regex(@"\'(.*)\'")), VoidResult);
    
    public static readonly AlternativePattern ptrDuoMarkArea = new (
        new RegexPattern(
            new Regex(@"\""(.*)\""")), VoidResult);

    public static readonly AlternativePattern ptrRoundBraceContent = new (
        new RegexPattern(
            new Regex(@"(?<=\()(.*?)(?=\))")), VoidResult);

    public static readonly AlternativePattern ptrSquareBraceContent = new (
        new RegexPattern(
            new Regex(@"(?<=\[)(.*?)(?=\])")), VoidResult);

    public static readonly AlternativePattern ptrVerticalBraceContent = new (
        new RegexPattern(
            new Regex(@"(?<=\|)(.*?)(?=\|)")), VoidResult);

    public static readonly AlternativePattern ptrFigureBraceContent = new (
        new RegexPattern(
            new Regex(@"(?<=\{)(.*?)(?=\})")), VoidResult);

    public static readonly AlternativePattern ptrSingleMarkContent = new (
        new RegexPattern(
            new Regex(@"(?<=\')(.*?)(?=\')")), VoidResult);

    public static readonly AlternativePattern ptrDuoMarkContent = new (
        new RegexPattern(
            new Regex(@"(?<=\"")(.*?)(?=\"")")), VoidResult);
    
    public static readonly AlternativePattern ptrMarksArea = new (
        new AnyPattern(ptrSingleMarkArea, ptrDuoMarkArea), VoidResult);
    
    public static readonly AlternativePattern ptrMarksContent = new (
        new AnyPattern(ptrSingleMarkContent, ptrDuoMarkContent), VoidResult);
    
    public static readonly Dictionary<string, Pattern[]> ptrEnumTags = new ()
    {
        {"radio", new Pattern[]{ptrRoundBraceArea, ptrRoundBraceContent}},
        {"checkbox", new Pattern[]{ptrSquareBraceArea, ptrSquareBraceContent}}
    };
    
    public static readonly Dictionary<string, string> tagClassNames = new ()
    {
        {"form", "template-form"},
        {"input", "template-input"},
        {"textarea", "template-textarea"},
        {"select", "template-select"},
        {"radio", "template-radio"},
        {"checkbox", "template-checkbox"},
    };

    /*private readonly IDatabase _redis;
    
    public TemplateParser(IConnectionMultiplexer muxer)
    {
        _redis = muxer.GetDatabase();
    }*/
    
    public async Task<string> ParseAsync(string markdown)
    {
        TemplateBuilder templateHTML = new();
        TemplateBuilder tempBuilder = new();
        StringBuilder content = new();
        List<string> keys = new();
        string tag, type, title, text;
        string literalKey, litrealBody;
        string redisValue, strHashCode;
        string[] literalParts = null;
        string[] options = null;
        int id = 0;
        
        templateHTML.AddTag("form").AddAttribute("class", tagClassNames["form"]);
        try
        {
            var literals = markdown
                .Split(';')
                .Where(literal => !string.IsNullOrWhiteSpace(literal))
                .Select(literal => literal.Trim())
                .ToArray();

            foreach (var literal in literals)
            {
                /*strHashCode = GetStableHashCode(literal);
                redisValue = await _redis.StringGetAsync(strHashCode);
                if (!String.IsNullOrEmpty(redisValue))
                {
                    templateHTML.AddText(redisValue);
                    continue;
                }*/
                
                literalParts = literal.Split(':');
                literalKey = Guid.NewGuid().ToString();
                title = literalParts[0].Trim();
                litrealBody = literalParts[1].Trim();
                
                if (keys.Contains(title))
                    throw new KeyExistingException($"The key ${title} already exist");
                
                keys.Append(literalKey);
                
                content.Clear();
                tempBuilder.Clear();
                tempBuilder.AddTag("div");

                if (!IsNull(ptrMarksArea.Execute(litrealBody, 0)?.Result))
                {
                    var markGroup = ptrMarkGroupWords.Matches(litrealBody);
                    tag = markGroup.Count > 1 ? "textarea" : "input";

                    foreach (Match match in markGroup) 
                    { 
                        text = ptrMarksContent.Execute(match.Value, 0).Result;
                        content.Append(string.IsNullOrEmpty(text) ? "\n" : $"{text}\n");
                    }

                    tempBuilder
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
                        tempBuilder
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
                            if (!IsNull(ptrVerticalBraceArea.Execute(option, 0)?.Result))
                            {
                                var optionTemplate = ptrVerticalBraceContent.Execute(option, 0);
                                tempBuilder
                                    .AddTag("option")
                                    .AddAttribute("value", optionTemplate.Result)
                                    .AddText(optionTemplate.Result)
                                    .AddTag("/option");
                            }
                        }
                        tempBuilder.AddTag("/select");
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
                                    .Substring(temp.EndPosition + 1)
                                    .Trim();

                                tempBuilder
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
                templateHTML.AddText(tempBuilder.AddTag("/div").Build());
                /*await _redis.StringAppendAsync(strHashCode,tempBuilder.Build());
                await _redis.KeyExpireAsync(strHashCode, TimeSpan.FromSeconds(1800));*/
            }
            
            return templateHTML.AddTag("/form").Build();
        }
        catch (Exception e)
        {
            tempBuilder.Clear();
            tempBuilder
                .AddTag("div")
                .AddAttribute("class", "alert alert-danger")
                .AddAttribute("role", "alert")
                .AddText(e.Message)
                .AddTag("/div");

            return tempBuilder.Build();
        }
    }

    private void AddTextInputTag(TemplateBuilder builder, string literalKey, string title, string content)
    {
        builder
            .AddTag("label")
            .AddText(title)
            .AddTag("input")
            .AddAttribute("type", "text")
            .AddAttribute("name", literalKey)
            .AddAttribute("id", literalKey)
            .AddAttribute("placeholder", title)
            .AddAttribute("value", content)
            .AddTag("/label");
    }

    private void AddTextareaTag(TemplateBuilder builder, string literalKey, string title, string content)
    {
        builder
            .AddTag("label")
            .AddText(title)
            .AddTag("textarea")
            .AddAttribute("name", literalKey)
            .AddAttribute("id", literalKey)
            .AddAttribute("placeholder", title)
            .AddText(content)
            .AddTag("/label");
    }

    private void AddSelectTag(TemplateBuilder builder, string literalKey, string title, string content)
    {
        
    }

    private void AddEnumTag(TemplateBuilder builder, string tag)
    {
        var i = 1;
    }

    private bool IsNull(object? value)
    {
        return value == null;
    }
    
    private string GetStableHashCode(string str)
    {
        unchecked
        {
            int hash1 = 5381;
            int hash2 = hash1;

            for(int i = 0; i < str.Length && str[i] != '\0'; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1 || str[i+1] == '\0')
                    break;
                hash2 = ((hash2 << 5) + hash2) ^ str[i+1];
            }

            return (hash1 + (hash2*1566083941)).ToString();
        }
    }
    
    private class KeyExistingException : Exception
    { 
        public KeyExistingException(string message) 
            : base(message) { }
    }
}