using System.Text;

namespace Templater.Services.MarkdownTemplateService.Builder;

public class TemplateBuilder
{
    private readonly StringBuilder template;
    
    public TemplateBuilder() => template = new StringBuilder(100);
    public TemplateBuilder(string head) => template = new StringBuilder(head, 100);
    public string Build() => template.ToString();
    
    public TemplateBuilder AddTag(string tag)
    {
        template.Append($"<{tag}>\n");
        return this;
    }

    public TemplateBuilder AddText(string text)
    {
        template.Append($"{text}\n");
        return this;
    }

    public TemplateBuilder AddAttribute(string name, string value)
    {
        var pos = template.ToString().LastIndexOf('>');
        template.Insert(pos, $@" {name}=""{value}""");
        return this;
    }
    
    public TemplateBuilder AddAttribute(string value)
    {
        var pos = template.ToString().LastIndexOf('>');
        template.Insert(pos, $" {value}");
        return this;
    }
    
    public TemplateBuilder Clear()
    {
        template.Clear();
        return this;
    }
}