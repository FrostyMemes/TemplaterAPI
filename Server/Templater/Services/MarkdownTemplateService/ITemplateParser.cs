namespace Templater.Services.MarkdownTemplateService;

public interface ITemplateParser
{
    Task<String> Parse(string markdown);
}