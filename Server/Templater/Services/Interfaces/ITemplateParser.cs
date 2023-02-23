namespace Templater.Services.MarkdownTemplateService;

public interface ITemplateParser
{
    Task<String> ParseAsync(string markdown);
}