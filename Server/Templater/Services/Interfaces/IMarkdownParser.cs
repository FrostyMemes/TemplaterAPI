namespace Templater.Services.MarkdownTemplateService;

public interface IMarkdownParser
{
    Task<String> ParseAsync(string markdown);
}