using Microsoft.AspNetCore.Mvc;
using Templater.Services.MarkdownTemplateService;

namespace Templater.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarkdownController : ControllerBase
{
    private readonly IMarkdownParser _markdownParser;

    public MarkdownController(IMarkdownParser markdownParser)
    {
        _markdownParser = markdownParser;
    }

    [HttpGet]
    public async Task<string> Get(string markdown)
    {
        var result = await _markdownParser.ParseAsync(markdown);
        return result;
    }
}