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
    static readonly PatternResult NoPatternResult = new (null, -1);
    
    public static readonly Regex ptrMarkGroupWords = new (
        @"""([^""\\]*(\\.[^""\\]*)*)""|\'([^\'\\]*(\\.[^\'\\]*)*)\'");
    
    public static readonly AlternativePattern ptrSquareBraceArea = new (
        new RegexPattern(
            new Regex(@"\[(.*)\]")), NoPatternResult);

    public static readonly AlternativePattern ptrRoundBraceArea = new (
        new RegexPattern(
            new Regex(@"\((.*)\)")), NoPatternResult);

    public static readonly AlternativePattern ptrVerticalBraceArea = new (
        new RegexPattern(
            new Regex(@"\|(.*)\|")), NoPatternResult);
    
    public static readonly AlternativePattern ptrSingleMarkArea = new (
        new RegexPattern(
            new Regex(@"\'(.*)\'")), NoPatternResult);
    
    public static readonly AlternativePattern ptrDuoMarkArea = new (
        new RegexPattern(
            new Regex(@"\""(.*)\""")), NoPatternResult);

    public static readonly AlternativePattern ptrRoundBraceContent = new (
        new RegexPattern(
            new Regex(@"(?<=\()(.*?)(?=\))")), NoPatternResult);

    public static readonly AlternativePattern ptrSquareBraceContent = new (
        new RegexPattern(
            new Regex(@"(?<=\[)(.*?)(?=\])")), NoPatternResult);

    public static readonly AlternativePattern ptrVerticalBraceContent = new (
        new RegexPattern(
            new Regex(@"(?<=\|)(.*?)(?=\|)")), NoPatternResult);

    public static readonly AlternativePattern ptrFigureBraceContent = new (
        new RegexPattern(
            new Regex(@"(?<=\{)(.*?)(?=\})")), NoPatternResult);

    public static readonly AlternativePattern ptrSingleMarkContent = new (
        new RegexPattern(
            new Regex(@"(?<=\')(.*?)(?=\')")), NoPatternResult);

    public static readonly AlternativePattern ptrDuoMarkContent = new (
        new RegexPattern(
            new Regex(@"(?<=\"")(.*?)(?=\"")")), NoPatternResult);
    
    public static readonly AlternativePattern ptrMarksArea = new (
        new AnyPattern(ptrSingleMarkArea, ptrDuoMarkArea), NoPatternResult);
    
    public static readonly AlternativePattern ptrMarksContent = new (
        new AnyPattern(ptrSingleMarkContent, ptrDuoMarkContent), NoPatternResult);
    
    public static readonly Dictionary<string, Pattern[]> ptrEnumTags = new ()
    {
        {"radio", new Pattern[]{ptrRoundBraceArea, ptrRoundBraceContent}},
        {"checkbox", new Pattern[]{ptrSquareBraceArea, ptrSquareBraceContent}}
    };
    
    /*private readonly IDatabase _redis;
    
    public TemplateParser(IConnectionMultiplexer muxer)
    {
        _redis = muxer.GetDatabase();
    }*/
    
    public async Task<string> ParseAsync(string markdown)
    {
        var templateHTML = new TemplateBuilder();
        var render = new StringBuilder();
        var content = new StringBuilder();
        var keys = new List<string>();
        string tag, type, text, id;
        string literalKey, litrealBody;
        string redisValue, strHashCode;
        string[] literalParts = null;
        string[] options = null;
        
        templateHTML.AddTag("form").AddAttribute("class", "template-form");
        
        try
        {
            var literals = GetLiterals(markdown).ToArray();

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
                literalKey = literalParts[0].Trim();
                litrealBody = literalParts[1].Trim();
                
                if (keys.Contains(literalKey))
                    throw new KeyExistingException($"The key ${literalKey} already exist");
                
                keys.Add(literalKey);
                id = Guid.NewGuid().ToString();

                render.Clear();
                content.Clear();

                if (!IsNull(ptrMarksArea.Execute(litrealBody, 0)?.Result))
                {
                    var markGroup = ptrMarkGroupWords.Matches(litrealBody);
                    tag = markGroup.Count > 1 ? "textarea" : "input";

                    foreach (Match match in markGroup) 
                    { 
                        text = ptrMarksContent.Execute(match.Value, 0).Result;
                        content.Append(string.IsNullOrEmpty(text) ? "" : $"{text}\n");
                    }
                    
                    render.Append(tag.Equals("input") 
                        ? RenderTextInputTag(literalKey, id, content.ToString()) 
                        : RenderTextareaTag(literalKey, id, content.ToString()));
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
                        render.Append(RenderSelectTag(literalKey, id, options));
                    }
                    else
                    {
                        type = IsNull(ptrRoundBraceArea.Execute(options[0], 0).Result)
                            ? "checkbox"
                            : "radio";
                        
                        render.Append(RenderEnumTag(type, literalKey, id, options));
                    }
                }
                templateHTML
                    .AddTag("div").AddAttribute("class","template-element")
                        .AddText(render.ToString())
                    .AddTag("/div");
                
                /*await _redis.StringAppendAsync(strHashCode,tempBuilder.Build());
                await _redis.KeyExpireAsync(strHashCode, TimeSpan.FromSeconds(1800));*/
            }
            
            return templateHTML.AddTag("/form").Build();
        }
        catch (Exception e)
        {
            templateHTML.Clear();
            templateHTML
                .AddTag("div").AddAttribute("class", "template-error")
                    .AddText(e.Message)
                .AddTag("/div");

            return templateHTML.Build();
        }
    }

    private IEnumerable<string> GetLiterals(string markdown)
    {
        return markdown.Split(';')
            .Where(literal => !string.IsNullOrWhiteSpace(literal))
            .Select(literal => literal.Trim());
    }

    private string RenderTextInputTag(string literalKey, string id, string content)
    {
        TemplateBuilder builder = new();
        
        return builder
            .AddTag("div").AddAttribute("class", "template-input-element")
                .AddTag("label").AddAttribute("for", id).AddAttribute("class", "template-label")
                    .AddText(literalKey)
                .AddTag("/label")
                .AddTag("input").AddAttribute("type", "text").AddAttribute("name", id)
                .AddAttribute("id", id).AddAttribute("value", content)
            .AddTag("/div")
            .Build();
    }

    private string RenderTextareaTag(string literalKey, string id, string content)
    {
        TemplateBuilder builder = new();
        return builder
            .AddTag("div").AddAttribute("class", "template-textarea-element")
                .AddTag("label").AddAttribute("for", id).AddAttribute("class", "template-label")
                    .AddText(literalKey)
                .AddTag("/label")
                .AddTag("textarea").AddAttribute("id", id).AddAttribute("name", id)
                .AddAttribute("class", "template-textarea")
                    .AddText(content)
                .AddTag("/textarea")
            .AddTag("/div")
            .Build();
    }

    private string RenderSelectTag(string literalKey, string id, string[] options)
    {
        TemplateBuilder builder = new();
        builder
            .AddTag("div").AddAttribute("class", "template-select-element")
                .AddTag("label").AddAttribute("for", id).AddAttribute("class", "template-label")
                    .AddText(literalKey)
                .AddTag("/label")
            .AddTag("select").AddAttribute("id", id).AddAttribute("name", id);

        foreach (var option in options) 
        {
            if (!IsNull(ptrVerticalBraceArea.Execute(option, 0)?.Result))
            {
                var optionTemplate = ptrVerticalBraceContent.Execute(option, 0);
                builder
                    .AddTag("option").AddAttribute("class","template-option")
                        .AddText(optionTemplate.Result)
                    .AddTag("/option");
            }
        }
        return builder
            .AddTag("/select").AddTag("/div")
            .Build();
    }

    private string RenderEnumTag(string type, string literalKey, string id, string[] options)
    {
        TemplateBuilder builder = new();
        var num = 1;

        builder.AddTag("div")
            .AddAttribute("id", $"id_{id}")
            .AddAttribute("class", $"template-{type}-element")
            .AddTag("label").AddAttribute("for", $"id_{id}").AddAttribute("class", "template-label")
            .AddText(literalKey)
            .AddTag("/label");

            
        
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

                builder
                    .AddTag("div").AddAttribute("class", $"template-{type}")
                        .AddTag("input")
                        .AddAttribute("type", type)
                        .AddAttribute("id", $"{num}_{id}")
                        .AddAttribute("name", id)
                        .AddAttribute(check)
                        .AddTag("/input")
                        .AddTag("label")
                        .AddAttribute("for", $"{num}_{id}")
                        .AddAttribute("class", $"{type}-label")
                        .AddText(optionLabel)
                        .AddTag("/label")
                    .AddTag("/div");
                num++;
            }
        }
        return builder.AddTag("/div").Build();
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
    
    private bool IsNull(object? value)
    {
        return value == null;
    }
    
    private class KeyExistingException : Exception
    { 
        public KeyExistingException(string message) 
            : base(message) { }
    }
}